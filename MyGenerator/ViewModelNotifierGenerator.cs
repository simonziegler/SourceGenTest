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

        /// <summary>
        /// The name of the attribute used by this generator
        /// </summary>
        public const string PolymorphicRecordAttributeName = "PolymorphicRecord";

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
    
    /// <summary>
    /// Used to discriminate derived types of the marked class. Not intended to be used for abstract classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class {PolymorphicRecordAttributeName}Attribute : System.Attribute {{ }}
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
            INamedTypeSymbol polymorphicRecordAttributeSymbol = compilation.GetTypeByMetadataName($"Vectis.Generator.{PolymorphicRecordAttributeName}Attribute");

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

                var isPolymorphicRecord = recordSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == polymorphicRecordAttributeSymbol.Name);
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
            string leadingTrivia = recordDeclarationSyntax.GetLeadingTrivia().ToString();
            List<(PropertyDeclarationSyntax syntax, IPropertySymbol symbol)> propertySymbols = new List<(PropertyDeclarationSyntax syntax, IPropertySymbol symbol)>();

            foreach (var property in properties)
            {
                SemanticModel model = compilation.GetSemanticModel(property.SyntaxTree);
                propertySymbols.Add((property, model.GetDeclaredSymbol(property)));
            }

            // begin building the generated source
            StringBuilder source = new StringBuilder();
            source.AppendLinesIndented(0, "using System;");
            source.AppendLinesIndented(0, "using System.Collections.Generic;");
            source.AppendLinesIndented(0, "using System.ComponentModel.DataAnnotations;");
            source.AppendLinesIndented(0, "using System.Linq;");
            source.AppendLinesIndented(0, "using Vectis.Events;");
            source.AppendLinesIndented(0, "");
            source.AppendLinesIndented(0, $"namespace {namespaceName}");
            source.AppendLinesIndented(0, "{");

            ProcessPartialRecordAddons();

            source.AppendLinesIndented(1, "");

            ProcessCompanionClass();

            source.AppendLinesIndented(1, "}");
            source.AppendLinesIndented(0, "}");

            return source;

            // Adds functionality to the record.
            void ProcessPartialRecordAddons()
            {
                source.AppendLinesIndented(1, leadingTrivia, true);

                if (recordSymbol.GetAttributes().Any(ad => ad.AttributeClass.Name == $"{PolymorphicRecordAttributeName}Attribute"))
                {
                    var children = GetChildRecords(recordSymbol);

                    for (int i = 0; i < children.Length; i++)
                    {
                        source.AppendLinesIndented(1, $"[MessagePack.Union({i}, typeof({children[i].Name}))]");
                    }
                }

                source.AppendLinesIndented(1, $"public {(recordSymbol.IsAbstract ? "abstract " : "")}partial record {recordSymbol.Name}");
                source.AppendLinesIndented(1, "{");

                // Build GetPropertyValuePairs()
                {
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Returns a list of <see cref=\"CreateObjectEvent.PropertyValuePair\"/> for each property of the record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"private protected {(isDerived ? "override" : "virtual")} List<CreateObjectEvent.PropertyValuePair> GetPropertyValuePairs()");
                    source.AppendLinesIndented(2, "{");
                    source.AppendLinesIndented(3, "List<CreateObjectEvent.PropertyValuePair> properties = new();");

                    if (isDerived)
                    {
                        source.AppendLinesIndented(3, "properties.AddRange(base.GetPropertyValuePairs());");
                    }

                    foreach (var propertySymbol in propertySymbols.Select(p => p.symbol))
                    {
                        source.AppendLinesIndented(3, $"properties.Add(new() {{ PropertyName = \"{propertySymbol.Name}\", Value = $\"{{{propertySymbol.Name}}}\" }});");
                    }

                    source.AppendLinesIndented(3, "return properties;");
                    source.AppendLinesIndented(2, "}");
                }

                // Build ApplyUpdatePropertyEvent()
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Applies an <see cref=\"UpdatePropertyEvent\" /> returning a new a record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"updatePropertyEvent\"></param>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"public {(isDerived ? "override" : "virtual")} ViewModelBase ApplyUpdatePropertyEvent(UpdatePropertyEvent updatePropertyEvent) => ApplyUpdatePropertyEvent(updatePropertyEvent, \"{recordSymbol.Name}\");");
                }

                // Build ApplyUpdatePropertyEvent(UpdatePropertyEvent updatePropertyEvent, string recordName)
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Applies an <see cref=\"UpdatePropertyEvent\" /> returning a new a record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"updatePropertyEvent\"></param>");
                    source.AppendLinesIndented(2, "/// <param name=\"recordName\"></param>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"private protected {(isDerived ? "override" : "virtual")} ViewModelBase ApplyUpdatePropertyEvent(UpdatePropertyEvent updatePropertyEvent, string recordName)");
                    source.AppendLinesIndented(2, "{");
                    source.AppendLinesIndented(3, $"var source = this{(isDerived ? "" : " with { EventId = updatePropertyEvent.Id }")};");
                    source.AppendLinesIndented(3, "");
                    source.AppendLinesIndented(3, "return updatePropertyEvent.PropertyName.ToLower() switch");
                    source.AppendLinesIndented(3, "{");

                    foreach (var propertySymbol in propertySymbols.Select(p => p.symbol))
                    {
                        AttributeData attributeData = propertySymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
                        TypedConstant readOnlyOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "ReadOnly").Value;

                        if (readOnlyOpt.IsNull || !Convert.ToBoolean(readOnlyOpt.Value))
                        {
                            source.AppendLinesIndented(4, $"\"{propertySymbol.Name.ToLower()}\" => source with {{ {propertySymbol.Name} = {GetPropertyUpdateSetter(propertySymbol, "updatePropertyEvent.NextValue")} }},");
                        }
                    }

                    if (isDerived)
                    {
                        source.AppendLinesIndented(4, "_ => base.ApplyUpdatePropertyEvent(updatePropertyEvent, recordName),");
                    }
                    else
                    {
                        source.AppendLinesIndented(4, $"_ => throw new Exception($\"Cannot set property '{{updatePropertyEvent.PropertyName}}' on record of type '{{recordName}}'\"),");
                    }

                    source.AppendLinesIndented(3, "};");
                    source.AppendLinesIndented(2, "}");
                }

                // Build ApplyDeleteObjectEvent() and ApplyUndeleteObjectEvent()
                if (!isDerived)
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Applies a <see cref=\"CreateObjectEvent\" /> returning a new a record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"deleteObjectEvent\"></param>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"public static ViewModelBase ApplyCreateObjectEvent(CreateObjectEvent createObjectEvent)");
                    source.AppendLinesIndented(2, "{");

                    var children = GetChildRecords(recordSymbol);

                    for (int i = 0; i < children.Length; i++)
                    {
                        var discriminatorAttribute = children[i].GetAttributes().Where(ad => ad.AttributeClass.Name == $"{TypeDiscriminatorAttributeName}Attribute").FirstOrDefault();
                        var discriminator = discriminatorAttribute.NamedArguments.SingleOrDefault(kvp => kvp.Key == "Discriminator").Value.Value.ToString();

                        source.AppendLinesIndented(3, $"if (createObjectEvent.TypeDiscriminator.ToLower() == \"{discriminator.ToLower()}\") return {children[i].Name}.ApplyTypedCreateObjectEvent(createObjectEvent);");
                    }

                    source.AppendLinesIndented(3, "throw new Exception($\"Cannot Create a record from a CreateObjectEvent containing a TypeDiscriminator of '{createObjectEvent.TypeDiscriminator}'\");");
                    source.AppendLinesIndented(2, "}");

                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Applies a <see cref=\"DeleteObjectEvent\" /> returning a new a record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"deleteObjectEvent\"></param>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"public ViewModelBase ApplyDeleteObjectEvent(DeleteObjectEvent deleteObjectEvent) => this with {{ EventId = deleteObjectEvent.Id, Deleted = true }};");

                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Applies a <see cref=\"UndeleteObjectEvent\" /> returning a new a record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"undeleteObjectEvent\"></param>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"public ViewModelBase ApplyUndeleteObjectEvent(UndeleteObjectEvent undeleteObjectEvent) => this with {{ EventId = undeleteObjectEvent.Id, Deleted = false }};");
                }

                // Build GetNotifier(), GetCreateObjectEvent(), and ApplyTypedCreateObjectEvent(CreateObjectEvent) only if the record is not abstract
                if (!recordSymbol.IsAbstract)
                {
                    // Build GetNotifier
                    {
                        source.AppendLinesIndented(2, $"");
                        source.AppendLinesIndented(2, $"/// <summary>");
                        source.AppendLinesIndented(2, $"/// Returns a <see cref=\"{GetNotifierClassName(recordSymbol.Name)}\"/> intialized with the same parameters held in this <see cref=\"{recordSymbol.Name}\"/>.");
                        source.AppendLinesIndented(2, $"/// </summary>");
                        source.AppendLinesIndented(2, $"/// <returns></returns>");
                        source.AppendLinesIndented(2, $"public {GetNotifierClassName(recordSymbol.Name)} GetNotifier()");
                        source.AppendLinesIndented(2, "{");
                        source.AppendLinesIndented(3, $"return new {GetNotifierClassName(recordSymbol.Name)}(this);");
                        source.AppendLinesIndented(2, "}");
                        source.AppendLinesIndented(2, "");
                    }

                    // Build GetCreateObjectEvent()
                    {
                        source.AppendLinesIndented(2, $"/// <summary>");
                        source.AppendLinesIndented(2, $"/// Returns a <see cref=\"CreateObjectEvent\"/> populated with the details in this record.");
                        source.AppendLinesIndented(2, $"/// </summary>");
                        source.AppendLinesIndented(2, $"/// <returns></returns>");
                        source.AppendLinesIndented(2, $"public CreateObjectEvent GetCreateObjectEvent()");
                        source.AppendLinesIndented(2, "{");
                        source.AppendLinesIndented(3, "return new()");
                        source.AppendLinesIndented(3, "{");
                        source.AppendLinesIndented(4, "Id = $\"{ViewModelEvent.NewId()}\",");
                        source.AppendLinesIndented(4, "UserId = $\"\",");
                        source.AppendLinesIndented(4, "IPAddress = $\"\",");
                        source.AppendLinesIndented(4, "Timestamp = DateTime.Now,");
                        source.AppendLinesIndented(4, $"TypeDiscriminator = \"{discriminatorString}\",");
                        source.AppendLinesIndented(4, "ObjectId = $\"{Id}\",");
                        source.AppendLinesIndented(4, "Properties = GetPropertyValuePairs().ToArray()");
                        source.AppendLinesIndented(3, "};");
                        source.AppendLinesIndented(2, "}");
                    }

                    // Build ApplyTypedCreateObjectEvent(CreateObjectEvent)
                    {
                        source.AppendLinesIndented(2, "");
                        source.AppendLinesIndented(2, $"/// <summary>");
                        source.AppendLinesIndented(2, $"/// Returns a <see cref=\"CreateObjectEvent\"/> populated with the details in this record.");
                        source.AppendLinesIndented(2, $"/// </summary>");
                        source.AppendLinesIndented(2, $"/// <returns></returns>");
                        source.AppendLinesIndented(2, $"internal static {recordSymbol.Name} ApplyTypedCreateObjectEvent(CreateObjectEvent createObjectEvent) => UpdateFromCreateObjectEvent(new(), createObjectEvent.Properties.ToList(), true).record;");
                    }
                }

                // Build ApplyTypedCreateObjectEvent(CreateObjectEvent, <current record>)
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"/// <summary>");
                    source.AppendLinesIndented(2, $"/// Returns a <see cref=\"CreateObjectEvent\"/> populated with the details in this record.");
                    source.AppendLinesIndented(2, $"/// </summary>");
                    source.AppendLinesIndented(2, $"/// <returns></returns>");
                    source.AppendLinesIndented(2, $"internal static ({recordSymbol.Name} record, List<CreateObjectEvent.PropertyValuePair> properties) UpdateFromCreateObjectEvent({recordSymbol.Name} record, List<CreateObjectEvent.PropertyValuePair> properties, bool isTargetRecord)");
                    source.AppendLinesIndented(2, "{");

                    if (isDerived)
                    {
                        source.AppendLinesIndented(3, $"var tuple = {recordSymbol.BaseType.Name}.UpdateFromCreateObjectEvent(record, properties, false);");
                        source.AppendLinesIndented(3, $"record = tuple.record as {recordSymbol.Name};");
                        source.AppendLinesIndented(3, $"properties = tuple.properties;");
                        source.AppendLinesIndented(3, "");
                    }

                    source.AppendLinesIndented(3, "foreach (var property in properties.ToList())");
                    source.AppendLinesIndented(3, "{");

                    var elseText = "";
                    foreach (var propertySymbol in propertySymbols.Select(p => p.symbol))
                    {
                        source.AppendLinesIndented(4, $"{elseText}if (property.PropertyName.ToLower() == \"{propertySymbol.Name.ToLower()}\")");
                        source.AppendLinesIndented(4, "{");
                        source.AppendLinesIndented(5, $"record = record with {{ {propertySymbol.Name} = {GetPropertyUpdateSetter(propertySymbol, "property.Value")} }};");
                        source.AppendLinesIndented(5, "properties.Remove(property);");
                        source.AppendLinesIndented(4, "}");
                        elseText = "else ";
                    }

                    source.AppendLinesIndented(4, $"else if (isTargetRecord) ");
                    source.AppendLinesIndented(4, "{");
                    source.AppendLinesIndented(5, $"throw new Exception($\"Cannot Create a '{recordSymbol.Name}' with a CreateObjectEvent containing a PropertyName of '{{property.PropertyName}}'\");");
                    source.AppendLinesIndented(4, "}"); source.AppendLinesIndented(3, "}");
                    source.AppendLinesIndented(3, "");
                    source.AppendLinesIndented(3, "return (record, properties);");
                    source.AppendLinesIndented(2, "}");
                }

                source.AppendLinesIndented(1, "}");
            }

            // Adds the companion class
            void ProcessCompanionClass()
            {
                source.AppendLinesIndented(1, $"/// <inheritdoc cref=\"{recordSymbol.Name}\"/>");
                source.AppendLinesIndented(1, $"/// <remarks>Companion edittable view model class to <see cref=\"{recordSymbol.Name}\"/>.</remarks>");
                source.AppendLinesIndented(1, $"public {(recordSymbol.IsAbstract ? "abstract " : "")}class {GetNotifierClassName(recordSymbol.Name)}{(isDerived ? " : " + GetNotifierClassName(recordSymbol.BaseType.Name) : "")}");
                source.AppendLinesIndented(1, "{");

                // Add the UpdatedEventHander delegate only if this is not a derived record (indicating that it's the base record)
                if (!isDerived)
                {
                    source.AppendLinesIndented(2, "/// <summary>");
                    source.AppendLinesIndented(2, "/// Represents method that will handle the <see cref=\"PropertyUpdated\"/> event raised by the view model in response to a property being updated.");
                    source.AppendLinesIndented(2, "/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"sender\"></param>");
                    source.AppendLinesIndented(2, "/// <param name=\"e\"></param>");
                    source.AppendLinesIndented(2, "public delegate void UpdatedPropertyEventHandler(object sender, ViewModelEvent e);");
                }

                if (!recordSymbol.IsAbstract)
                {
                    source.AppendLinesIndented(2, "/// <summary>");
                    source.AppendLinesIndented(2, "/// Occurs when a property is updated.");
                    source.AppendLinesIndented(2, "/// </summary>");
                    source.AppendLinesIndented(2, "/// <param name=\"sender\"></param>");
                    source.AppendLinesIndented(2, "/// <param name=\"e\"></param>");
                    source.AppendLinesIndented(2, "public event UpdatedPropertyEventHandler PropertyUpdated;");
                }

                // Add properties, including a copy of the originator record only if this is a non-abstract class
                {
                    if (!isDerived)
                    {
                        source.AppendLinesIndented(2, "");
                        source.AppendLinesIndented(2, "/// <summary>");
                        source.AppendLinesIndented(2, $"/// The backing <see cref=\"{recordSymbol.Name}\"/> from which this object was created.");
                        source.AppendLinesIndented(2, "/// </summary>");
                        source.AppendLinesIndented(2, $"public readonly ViewModelBase OriginatorRecord;");
                    }

                    // Create companion properties for each of the base record's fields
                    foreach (var propertySymbol in propertySymbols)
                    {
                        ProcessCompanionClassProperty(recordSymbol.Name, propertySymbol.syntax, propertySymbol.symbol);
                    }
                }

                // Build the constructor which calls the base constructor if this is a derived record
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, $"public {GetNotifierClassName(recordSymbol.Name)}({recordSymbol.Name} record){(isDerived ? " : base(record)" : "")}");
                    source.AppendLinesIndented(2, "{");

                    if (!isDerived)
                    {
                        source.AppendLinesIndented(3, $"OriginatorRecord = record;");
                    }

                    foreach (var propertySymbol in propertySymbols.Select(p => p.symbol))
                    {
                        source.AppendLinesIndented(3, $"{propertySymbol.Name} = record.{propertySymbol.Name};");
                    }

                    source.AppendLinesIndented(2, "}");
                }

                // Build GetRecord()
                if (!recordSymbol.IsAbstract)
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, "/// <summary>");
                    source.AppendLinesIndented(2, $"/// Builds a <see cref=\"{recordSymbol.Name}\"/> copying the values from this class.");
                    source.AppendLinesIndented(2, "/// </summary>");
                    source.AppendLinesIndented(2, "/// <returns></returns>");
                    source.AppendLinesIndented(2, $"public {recordSymbol.Name} GetRecord() => GetRecord(new {recordSymbol.Name}());");
                }

                // Build GetTypedRecord<T>()
                {
                    source.AppendLinesIndented(2, "");
                    source.AppendLinesIndented(2, "/// <summary>");
                    source.AppendLinesIndented(2, $"/// Builds a record of type T copying the values from this class.");
                    source.AppendLinesIndented(2, "/// </summary>");
                    source.AppendLinesIndented(2, "/// <returns></returns>");
                    source.AppendLinesIndented(2, $"private protected {recordSymbol.Name} GetRecord({recordSymbol.Name} record)");//{(isDerived ? "override" : "virtual")} 
                    source.AppendLinesIndented(2, "{");

                    if (isDerived)
                    {
                        source.AppendLinesIndented(3, $"return (base.GetRecord(record) as {recordSymbol.Name}) with");
                    }
                    else
                    {
                        source.AppendLinesIndented(3, $"return (record  as {recordSymbol.Name}) with");
                    }

                    source.AppendLinesIndented(3, "{");

                    foreach (var propertySymbol in propertySymbols.Select(p => p.symbol))
                    {
                        source.AppendLinesIndented(4, $"{propertySymbol.Name} = this.{propertySymbol.Name},");
                    }

                    source.AppendLinesIndented(3, "};");
                    source.AppendLinesIndented(2, "}");
                }

                return;

                // Adds a property to the companion class
                void ProcessCompanionClassProperty(string recordName, PropertyDeclarationSyntax propertyDeclarationSyntax, IPropertySymbol propertySymbol)
                {
                    // get the name and type of the field
                    ITypeSymbol fieldType = propertySymbol.Type;
                    var fieldName = "_" + propertySymbol.Name.Substring(0, 1).ToLower() + propertySymbol.Name.Substring(1);

                    // get the ViewModel attribute from the field, and any associated data
                    AttributeData attributeData = propertySymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
                    TypedConstant readOnlyOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "ReadOnly").Value;

                    source.AppendLinesIndented(2, "");
                    if (readOnlyOpt.IsNull || !Convert.ToBoolean(readOnlyOpt.Value))
                    {
                        source.AppendLinesIndented(2, $"private {fieldType} {fieldName};");
                        source.AppendLinesIndented(2, $"/// <inheritdoc cref=\"{recordName}.{propertySymbol.Name}\" />");
                        
                        foreach (var attribute in propertyDeclarationSyntax.AttributeLists.ToList().Where(attr => attr.ToString().Substring(1, ViewModelAttributeName.Length) != ViewModelAttributeName))
                        {
                            source.AppendLinesIndented(2, attribute.ToString());
                        }

                        source.AppendLinesIndented(2, $"public {fieldType} {propertySymbol.Name}");
                        source.AppendLinesIndented(2, "{");
                        source.AppendLinesIndented(3, $"get => {fieldName};");
                        source.AppendLinesIndented(3, "set");
                        source.AppendLinesIndented(3, "{");
                        source.AppendLinesIndented(4, $"if ({fieldName} != value)");
                        source.AppendLinesIndented(4, "{");
                        source.AppendLinesIndented(5, $"var updatePropertyEvent = OriginatorRecord.BuildUpdatePropertyEvent(\"{propertySymbol.Name}\", {fieldName}, value);");
                        source.AppendLinesIndented(5, $"{fieldName} = value;");
                        source.AppendLinesIndented(5, $"PropertyUpdated?.Invoke(this, updatePropertyEvent);");
                        source.AppendLinesIndented(4, "}");
                        source.AppendLinesIndented(3, "}");
                        source.AppendLinesIndented(2, "}");
                    }
                    else
                    {
                        source.AppendLinesIndented(2, $"/// <inheritdoc cref=\"{recordName}.{propertySymbol.Name}\" />");
                        source.AppendLinesIndented(2, $"public readonly {fieldType} {propertySymbol.Name};");
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


        private INamespaceOrTypeSymbol[] GetChildRecords(INamedTypeSymbol recordSymbol)
        {
            var namespaceCompilation = CSharpCompilation.Create(recordSymbol.ContainingAssembly.Name);
            var namespaceMembers = recordSymbol.ContainingNamespace.GetMembers();

            return namespaceMembers
                .Where(c => namespaceCompilation.ClassifyConversion(recordSymbol, c as ITypeSymbol).IsExplicit)
                .Where(c => c.GetAttributes().Any(ad => ad.AttributeClass.Name == $"{TypeDiscriminatorAttributeName}Attribute"))
                .ToArray();
        }


        /// <summary>
        /// Returns the class name for the view notifier associated with a view model record.
        /// </summary>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public static string GetNotifierClassName(string recordName) => recordName + "ViewNotifier";


        /// <summary>
        /// Returns the class name for the view notifier associated with a view model record.
        /// </summary>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public static string GetPropertyUpdateSetter(IPropertySymbol propertySymbol, string propertyName)
        {
            var typeName = propertySymbol.Type.Name;

            if (typeName == "String") return propertyName;
            return $"Convert.To{typeName}({propertyName})";
        }
    }
}

