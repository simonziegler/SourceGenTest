using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyGenerator
{
    [Generator]
    public class ViewModelNotifierGenerator : ISourceGenerator
    {
        /// <summary>
        /// Tjhe name of the attribute used by this generator
        /// </summary>
        public const string AttributeName = "ViewModel";
        private static int crap = 0;
        private static readonly string attributeText = $@"
using System;
namespace Vectis.Generator
{{
    /// <summary>
    /// Causes the vectis source generator to mark this as a property to include in a notifier to emit update events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class {AttributeName}Attribute : Attribute
    {{
        public string PropertyName {{ get; set; }}
        
        public {AttributeName}Attribute() {{ }}
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
            context.AddSource($"{AttributeName}Attribute", attributeText);

            // retreive the populated receiver 
            if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
                return;

            // we're going to create a new compilation that contains the attribute.
            // TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
            CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

            // get the newly bound attribute, and INotifyPropertyChanged
            INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName($"Vectis.Generator.{AttributeName}Attribute");
            INamedTypeSymbol notifySymbol = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

            List<IPropertySymbol> propertySymbols = new List<IPropertySymbol>();
            foreach (PropertyDeclarationSyntax property in receiver.CandidateProperties)
            {
                SemanticModel model = compilation.GetSemanticModel(property.SyntaxTree);
                IPropertySymbol propertySymbol = model.GetDeclaredSymbol(property) as IPropertySymbol;
                propertySymbols.Add(propertySymbol);
            }

            // group the fields by class, and generate the source
            foreach (var group in propertySymbols.GroupBy(f => f.ContainingType))
            {
                // Respectively the type symbol cannot be abstract, public, a record (established by the presence of an EqualityContract and partial
                //if (!group.Key.IsAbstract
                //    && group.Key.DeclaredAccessibility == Accessibility.Public
                //    && group.Key.GetMembers().Any(x => x.Kind == SymbolKind.Property && x.Name == "EqualityContract" && x.IsImplicitlyDeclared)
                //    && true)
                {
                    string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, context);
                    context.AddSource($"{GetNotifierClassName(group.Key.Name)}.cs", classSource);
                }
            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, List<IPropertySymbol> properties, ISymbol attributeSymbol, ISymbol notifySymbol, GeneratorExecutionContext context)
        {
            //return null;
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                return null; //TODO: issue a diagnostic that it must be top level
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            
            string abstractTag = "";

            if (classSymbol.IsAbstract)
            {
                abstractTag = "abstract ";
            }

            string baseInheritance = "";
            string baseConstructorCall = "";

            if (classSymbol.BaseType != null && classSymbol.BaseType.ContainingNamespace.Name == classSymbol.ContainingNamespace.Name)
            {
                baseInheritance = $"{GetNotifierClassName(classSymbol.BaseType.Name)}, ";
                baseConstructorCall = " : base(record)";
            }

            // begin building the generated source
            StringBuilder source = new StringBuilder();
            source.AppendLineIndented(0, $"namespace {namespaceName}");
            source.AppendLineIndented(0, "{");
            source.AppendLineIndented(1, $"public partial record {classSymbol.Name}");
            source.AppendLineIndented(1, "{");
            source.AppendLineIndented(2, $"public string Crap{crap++} {{ get; init; }}");
            source.AppendLineIndented(1, "}");
            source.AppendLineIndented(1, "");
            source.AppendLineIndented(1, "");
            source.AppendLineIndented(1, $"public {abstractTag}class {GetNotifierClassName(classSymbol.Name)} : {baseInheritance}{notifySymbol.ToDisplayString()}");
            source.AppendLineIndented(1, "{");
            source.AppendLineIndented(2, "public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
            source.AppendLineIndented(2, "");
            source.AppendLineIndented(2, $"public {GetNotifierClassName(classSymbol.Name)}({classSymbol.Name} record){baseConstructorCall}");
            source.AppendLineIndented(2, "{");

            foreach (var propertySymbol in properties)
            {
                source.AppendLineIndented(3, $"this.{propertySymbol.Name} = record.{propertySymbol.Name};");
            }

            source.AppendLineIndented(2, "}");
            source.AppendLineIndented(2, "");

            // create properties for each field 
            foreach (var propertySymbol in properties)
            {
                ProcessProperty(source, classSymbol.Name, propertySymbol, attributeSymbol);
            }

            source.AppendLineIndented(1, "}");
            source.AppendLineIndented(0, "}");
            
            return source.ToString();
        }

        private void ProcessProperty(StringBuilder source, string recordName, IPropertySymbol propertySymbol, ISymbol attributeSymbol)
        {
            // get the name and type of the field
            ITypeSymbol fieldType = propertySymbol.Type;
            var fieldName = "_" + propertySymbol.Name.Substring(0, 1).ToLower() + propertySymbol.Name.Substring(1);

            // get the ViewModel attribute from the field, and any associated data
            AttributeData attributeData = propertySymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            TypedConstant overridenNameOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

            source.AppendLineIndented(2, "");
            source.AppendLineIndented(2, $"private {fieldType} {fieldName};");
            source.AppendLineIndented(2, $"/// <inheritdoc cref=\"{recordName}.{propertySymbol.Name}\" />");
            source.AppendLineIndented(2, $"public {fieldType} {propertySymbol.Name}");
            source.AppendLineIndented(2, "{");
            source.AppendLineIndented(3, $"get => this.{fieldName};");
            source.AppendLineIndented(3, "set");
            source.AppendLineIndented(3, "{");
            source.AppendLineIndented(4, $"this.{fieldName} = value;");
            source.AppendLineIndented(4, $"this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertySymbol.Name})));");
            source.AppendLineIndented(3, "}");
            source.AppendLineIndented(2, "}");
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
                    && propertyDeclarationSyntax.AttributeLists.Where(al => al.ToString() == $"[{AttributeName}]").Any())
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

