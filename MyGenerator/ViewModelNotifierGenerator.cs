using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyGenerator
{
    [Generator]
    public class ViewModelNotifierGenerator : ISourceGenerator
    {
        /// <summary>
        /// The name of the attribute used by this generator to identify view model's base record.
        /// events.
        /// </summary>
        public const string ViewModelBaseRecordAttributeName = "ViewModelBaseRecord";

        /// <summary>
        /// The name of the attribute used by this generator to identify view model properties to be
        /// replicated from VM records to companion notifier classes and to be tied to database
        /// events.
        /// </summary>
        public const string ViewModelAttributeName = "ViewModel";

        /// <summary>
        /// The name of the attribute used by this generator
        /// </summary>
        public const string TypeDiscriminatorAttributeName = "TypeDiscriminator";

        private static readonly string attributeText = $@"
using System;
namespace Vectis.Generator
{{
    /// <summary>
    /// Attribute to be used on the root class of the view model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class {ViewModelBaseRecordAttributeName}Attribute : Attribute {{ }}
    
    /// <summary>
    /// Causes the vectis source generator to mark this as a property to include in a notifier to emit update events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class {ViewModelAttributeName}Attribute : Attribute
    {{
        /// <summary>
        /// Set to true this property will be readonly in its companion notifier.
        /// Used for ids and other immutable data.
        /// </summary>
        public bool ReadOnly {{ get; set; }} = false;
    }}
    
    /// <summary>
    /// Used to discriminate derived types of the marked class. Not intended to be used for abstract classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class {TypeDiscriminatorAttributeName}Attribute : System.Attribute
    {{
        /// <summary>
        /// The name of the property used to discriminate derived types of the class marked by this attribute.
        /// </summary>
        public string Discriminator {{ get; set; }}
    }}
}}
";

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUGx
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // add the attribute text
            context.AddSource($"{ViewModelAttributeName}Attribute", attributeText);

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            // we're going to create a new compilation that contains the attribute.
            // TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
            CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

            // get the newly bound attribute, and INotifyPropertyChanged
            INamedTypeSymbol viewModelAttributeSymbol = compilation.GetTypeByMetadataName($"Vectis.Generator.{ViewModelAttributeName}Attribute");
            INamedTypeSymbol typeDiscriminatorAttributeSymbol = compilation.GetTypeByMetadataName($"Vectis.Generator.{TypeDiscriminatorAttributeName}Attribute");
            
            List<(IPropertySymbol propertySymbol, RecordDeclarationSyntax recordDeclarationSyntax)> propertySymbols = new List<(IPropertySymbol, RecordDeclarationSyntax)>();
            foreach (var group in receiver.CandidateProperties.GroupBy(cp => cp.Parent))
            {
                var recordDeclarationSyntax = group.Key as RecordDeclarationSyntax;
                
                if (recordDeclarationSyntax == null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                            "VSG0001",
                            $"{ViewModelAttributeName} can only be used in records",
                            $"{ViewModelAttributeName} is reserved for use in public partial records and cannot be used in '{group.Key}', which is not a record",
                            "Vectis Source Generation",
                            DiagnosticSeverity.Error,
                            true
                        ), Location.None));

                    continue;
                }

                var modifiers = recordDeclarationSyntax.Modifiers.Select(m => m.Text).ToList();
                SemanticModel recordModel = compilation.GetSemanticModel(recordDeclarationSyntax.SyntaxTree);
                INamedTypeSymbol recordSymbol = recordModel.GetDeclaredSymbol(recordDeclarationSyntax);

                if (!modifiers.Contains("public") || !modifiers.Contains("partial"))
                {
                    context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                            "VSG0002",
                            $"{ViewModelAttributeName} can only be used in public records",
                            $"{ViewModelAttributeName} is reserved for use in public partial records and cannot be used in '{recordSymbol.Name}', which is not declared as 'public partial'",
                            "Vectis Source Generation",
                            DiagnosticSeverity.Error,
                            true
                        ), Location.None));

                    continue;
                }

                if (!modifiers.Contains("abstract") && !recordSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == typeDiscriminatorAttributeSymbol.Name))
                {
                    context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                            "VSG0003",
                            $"{ViewModelAttributeName} can only be used in public records that are decorated with the {TypeDiscriminatorAttributeName} attribute",
                            $"{ViewModelAttributeName} is reserved for use in public partial records that are decorated with the {TypeDiscriminatorAttributeName} attribute and cannot be used in '{recordSymbol.Name}', which lacks [{TypeDiscriminatorAttributeName}(\"... Unique Type Discrimination Text ...\")]",
                            "Vectis Source Generation",
                            DiagnosticSeverity.Error,
                            true
                        ), Location.None));

                    continue;
                }

                var discriminator = "";

                if (recordSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == typeDiscriminatorAttributeSymbol.Name))
                {
                    var attributeData = recordSymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(typeDiscriminatorAttributeSymbol, SymbolEqualityComparer.Default));
                    discriminator = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "Discriminator").Value.Value.ToString();
                }
                else if (!modifiers.Contains("abstract"))
                {
                    context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                            "VSG0004",
                            $"{TypeDiscriminatorAttributeName} missing 'Discriminator' value",
                            $"{TypeDiscriminatorAttributeName} requires a value for the 'Discriminator' property in '{recordSymbol.Name}', e.g. [{TypeDiscriminatorAttributeName}(\"... Unique Type Discrimination Text ...\")]",
                            "Vectis Source Generation",
                            DiagnosticSeverity.Error,
                            true
                        ), Location.None));

                    continue;
                }

                var generatedSource = ProcessRecordAndClass(recordSymbol, recordDeclarationSyntax, group.ToList(), viewModelAttributeSymbol, discriminator, context, compilation);
                
                context.AddSource($"{GetNotifierClassName(recordSymbol.Name)}.cs", generatedSource.ToString());
            }
        }

        /// <summary>
        /// Returns a StringBuilder with the contents of a generated source file for extension to a partial record plus its companion notifier class.
        /// </summary>
        /// <param name="recordSymbol"></param>
        /// <param name="recordDeclarationSyntax"></param>
        /// <param name="properties"></param>
        /// <param name="attributeSymbol"></param>
        /// <param name="notifySymbol"></param>
        /// <param name="discriminatorString"></param>
        /// <param name="context"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        private StringBuilder ProcessRecordAndClass(INamedTypeSymbol recordSymbol, RecordDeclarationSyntax recordDeclarationSyntax, List<PropertyDeclarationSyntax> properties, ISymbol attributeSymbol, string discriminatorString, GeneratorExecutionContext context, Compilation compilation)
        {
            string namespaceName = recordSymbol.ContainingNamespace.ToDisplayString();
            bool isDerived = recordSymbol.BaseType != null && recordSymbol.BaseType.ContainingNamespace.Name == recordSymbol.ContainingNamespace.Name;
            string leadingComment = recordDeclarationSyntax.GetLeadingTrivia().ToString().Trim();
            List<IPropertySymbol> propertySymbols = new List<IPropertySymbol>();

            foreach (var property in properties)
            {
                SemanticModel model = compilation.GetSemanticModel(property.SyntaxTree);
                propertySymbols.Add(model.GetDeclaredSymbol(property));
            }

            // begin building the generated source
            StringBuilder source = new StringBuilder();
            source.AppendLineIndented(0, "using System;");
            source.AppendLineIndented(0, "using System.Collections.Generic;");
            source.AppendLineIndented(0, "using Vectis.Events;");
            source.AppendLineIndented(0, "");
            source.AppendLineIndented(0, $"namespace {namespaceName}");
            source.AppendLineIndented(0, "{");

            ProcessPartialRecordAddons();

            source.AppendLineIndented(1, "");

            ProcessCompanionClass();

            source.AppendLineIndented(1, "}");
            source.AppendLineIndented(0, "}");

            return source;

            // Adds functionality to the record.
            void ProcessPartialRecordAddons()
            {
                if (!string.IsNullOrEmpty(leadingComment))
                {
                    source.AppendLineIndented(1, leadingComment);
                }

                source.AppendLineIndented(1, $"public partial record {recordSymbol.Name}");
                source.AppendLineIndented(1, "{");

                // Build GetPropertyValuePairs()
                {
                    source.AppendLineIndented(2, $"/// <summary>");
                    source.AppendLineIndented(2, $"/// Returns a list of <see cref=\"CreateObjectEvent.PropertyValuePair\"/> for each property of the record.");
                    source.AppendLineIndented(2, $"/// </summary>");
                    source.AppendLineIndented(2, $"/// <returns></returns>");
                    source.AppendLineIndented(2, $"internal {(isDerived ? "override" : "virtual")} List<CreateObjectEvent.PropertyValuePair> GetPropertyValuePairs()");
                    source.AppendLineIndented(2, "{");
                    source.AppendLineIndented(3, "List<CreateObjectEvent.PropertyValuePair> properties = new();");

                    if (isDerived)
                    {
                        source.AppendLineIndented(3, "properties.AddRange(base.GetPropertyValuePairs());");
                    }

                    foreach (var propertySymbol in propertySymbols)
                    {
                        source.AppendLineIndented(3, $"properties.Add(new() {{ PropertyName = \"{propertySymbol.Name}\", Value = $\"{{{propertySymbol.Name}}}\" }});");
                    }

                    source.AppendLineIndented(3, "return properties;");
                    source.AppendLineIndented(2, "}");
                }

                // Build GetNotifier() only if the record is not abstract
                if (!recordSymbol.IsAbstract)
                {
                    source.AppendLineIndented(2, $"");
                    source.AppendLineIndented(2, $"/// <summary>");
                    source.AppendLineIndented(2, $"/// Returns a <see cref=\"{GetNotifierClassName(recordSymbol.Name)}\"/> intialized with the same parameters held in this <see cref=\"{recordSymbol.Name}\"/>.");
                    source.AppendLineIndented(2, $"/// </summary>");
                    source.AppendLineIndented(2, $"/// <returns></returns>");
                    source.AppendLineIndented(2, $"public {GetNotifierClassName(recordSymbol.Name)} GetNotifier()");
                    source.AppendLineIndented(2, "{");
                    source.AppendLineIndented(3, $"return new {GetNotifierClassName(recordSymbol.Name)}(this);");
                    source.AppendLineIndented(2, "}");
                    source.AppendLineIndented(2, "");

                    source.AppendLineIndented(2, $"/// <summary>");
                    source.AppendLineIndented(2, $"/// Returns a <see cref=\"CreateObjectEvent\"/> populated with the details in this record.");
                    source.AppendLineIndented(2, $"/// </summary>");
                    source.AppendLineIndented(2, $"/// <returns></returns>");
                    source.AppendLineIndented(2, $"public CreateObjectEvent GetCreateObjectEvent(string userId)");
                    source.AppendLineIndented(2, "{");
                    source.AppendLineIndented(3, "return new()");
                    source.AppendLineIndented(3, "{");
                    source.AppendLineIndented(4, "Id = $\"{ViewModelEvent.NewId()}\",");
                    source.AppendLineIndented(4, "UserId = $\"{userId}\",");
                    source.AppendLineIndented(4, "Timestamp = DateTime.Now,");
                    source.AppendLineIndented(4, $"TypeDiscriminator = \"{discriminatorString}\",");
                    source.AppendLineIndented(4, "ObjectId = $\"{Id}\",");
                    source.AppendLineIndented(4, "Properties = GetPropertyValuePairs().ToArray()");
                    source.AppendLineIndented(3, "};");
                    source.AppendLineIndented(2, "}");
                }

                source.AppendLineIndented(1, "}");
            }

            // Adds the companion class
            void ProcessCompanionClass()
            {
                source.AppendLineIndented(1, $"/// <inheritdoc cref=\"{recordSymbol.Name}\"/>");
                source.AppendLineIndented(1, $"/// <remarks>Companion edittable view model class to <see cref=\"{recordSymbol.Name}\"/>.</remarks>");
                source.AppendLineIndented(1, $"public {(recordSymbol.IsAbstract ? "abstract " : "")}class {GetNotifierClassName(recordSymbol.Name)}{(isDerived ? " : " + GetNotifierClassName(recordSymbol.BaseType.Name) : "")}");
                source.AppendLineIndented(1, "{");

                // Add the UpdatedEventHander delegate only if this is not a derived record (indicating that it's the base record)
                if (!isDerived)
                {
                    source.AppendLineIndented(2, "/// <summary>");
                    source.AppendLineIndented(2, "/// The method that will handle an event raised by the view model in response to a property being updated.");
                    source.AppendLineIndented(2, "/// </summary>");
                    source.AppendLineIndented(2, "/// <param name=\"sender\"></param>");
                    source.AppendLineIndented(2, "/// <param name=\"e\"></param>");
                    source.AppendLineIndented(2, "public delegate void UpdatedEventHandler(object sender, ViewModelEvent e);");

                }

                // Add a copy of the originator record only if this is a non-abstract class
                {
                    if (!recordSymbol.IsAbstract)
                    {
                        source.AppendLineIndented(2, "");
                        source.AppendLineIndented(2, "/// <summary>");
                        source.AppendLineIndented(2, $"/// The backing <see cref=\"{recordSymbol.Name}\"/> from which this object was created.");
                        source.AppendLineIndented(2, "/// </summary>");
                        source.AppendLineIndented(2, $"public readonly {recordSymbol.Name} OriginatorRecord;");
                    }

                    // Create companion properties for each of the base record's fields
                    foreach (var propertySymbol in propertySymbols)
                    {
                        ProcessCompanionClassProperty(recordSymbol.Name, propertySymbol);
                    }
                }

                // Build the constructor which calls the base constructor if this is a derived record
                {
                    source.AppendLineIndented(2, "");
                    source.AppendLineIndented(2, $"public {GetNotifierClassName(recordSymbol.Name)}({recordSymbol.Name} record){(isDerived ? " : base(record)" : "")}");
                    source.AppendLineIndented(2, "{");

                    if (!recordSymbol.IsAbstract)
                    {
                        source.AppendLineIndented(3, $"OriginatorRecord = record;");
                    }

                    foreach (var propertySymbol in propertySymbols)
                    {
                        source.AppendLineIndented(3, $"{propertySymbol.Name} = record.{propertySymbol.Name};");
                    }

                    source.AppendLineIndented(2, "}");
                }

                // Build GetRecord()
                if (!recordSymbol.IsAbstract)
                {
                    source.AppendLineIndented(2, "");
                    source.AppendLineIndented(2, "/// <summary>");
                    source.AppendLineIndented(2, $"/// Builds a <see cref=\"{recordSymbol.Name}\"/> copying the values from this class.");
                    source.AppendLineIndented(2, "/// </summary>");
                    source.AppendLineIndented(2, "/// <returns></returns>");
                    source.AppendLineIndented(2, $"public {recordSymbol.Name} GetRecord() => GetTypedRecord<{recordSymbol.Name}>(new {recordSymbol.Name}());");
                }

                // Build GetTypedRecord<T>()
                {
                    source.AppendLineIndented(2, "");
                    source.AppendLineIndented(2, "/// <summary>");
                    source.AppendLineIndented(2, $"/// Builds a record of type T copying the values from this class.");
                    source.AppendLineIndented(2, "/// </summary>");
                    source.AppendLineIndented(2, "/// <returns></returns>");
                    source.AppendLineIndented(2, $"internal {(isDerived ? "override" : "virtual")} T GetTypedRecord<T>(T record)");
                    source.AppendLineIndented(2, "{");

                    if (isDerived)
                    {
                        source.AppendLineIndented(3, $"return (T)((base.GetTypedRecord<T>(record) as {recordSymbol.Name}) with");
                    }
                    else
                    {
                        source.AppendLineIndented(3, $"return (T)((record  as {recordSymbol.Name}) with");
                    }

                    source.AppendLineIndented(3, "{");

                    foreach (var propertySymbol in propertySymbols)
                    {
                        source.AppendLineIndented(4, $"{propertySymbol.Name} = this.{propertySymbol.Name},");
                    }

                    source.AppendLineIndented(3, "});");
                    source.AppendLineIndented(2, "}");
                }

                return;

                // Adds a property to the companion class
                void ProcessCompanionClassProperty(string recordName, IPropertySymbol propertySymbol)
                {
                    // get the name and type of the field
                    ITypeSymbol fieldType = propertySymbol.Type;
                    var fieldName = "_" + propertySymbol.Name.Substring(0, 1).ToLower() + propertySymbol.Name.Substring(1);

                    // get the ViewModel attribute from the field, and any associated data
                    AttributeData attributeData = propertySymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
                    TypedConstant readOnlyOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "ReadOnly").Value;

                    source.AppendLineIndented(2, "");
                    if (readOnlyOpt.IsNull || !Convert.ToBoolean(readOnlyOpt.Value))
                    {
                        source.AppendLineIndented(2, $"private {fieldType} {fieldName};");
                        source.AppendLineIndented(2, $"/// <inheritdoc cref=\"{recordName}.{propertySymbol.Name}\" />");
                        source.AppendLineIndented(2, $"public {fieldType} {propertySymbol.Name}");
                        source.AppendLineIndented(2, "{");
                        source.AppendLineIndented(3, $"get => {fieldName};");
                        source.AppendLineIndented(3, "set");
                        source.AppendLineIndented(3, "{");
                        source.AppendLineIndented(4, $"{fieldName} = value;");
                        source.AppendLineIndented(4, $"//UpdatedEventHandler?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertySymbol.Name})));");
                        source.AppendLineIndented(3, "}");
                        source.AppendLineIndented(2, "}");
                    }
                    else
                    {
                        source.AppendLineIndented(2, $"/// <inheritdoc cref=\"{recordName}.{propertySymbol.Name}\" />");
                        source.AppendLineIndented(2, $"public readonly {fieldType} {propertySymbol.Name};");
                    }
                }
            }
        }


        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<PropertyDeclarationSyntax> CandidateProperties { get; } = new List<PropertyDeclarationSyntax>();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                // any field with at least one attribute is a candidate for property generation
                if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax
                    && propertyDeclarationSyntax.AttributeLists.Where(al => al.ToString().Substring(1, ViewModelAttributeName.Length) == ViewModelAttributeName).Any())
                {
                    CandidateProperties.Add(propertyDeclarationSyntax);
                }
            }
        }


        /// <summary>
        /// Returns the class name for the view notifier associated with a view model record.
        /// </summary>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public static string GetNotifierClassName(string recordName) => recordName + "ViewNotifier";
    }
}

