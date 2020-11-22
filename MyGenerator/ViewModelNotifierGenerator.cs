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
        /// Tjhe name of the attribute used by this generator
        /// </summary>
        public const string AttributeName = "ViewModel";
        
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
#if DEBUG
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
                string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, context);
                context.AddSource($"{GetNotifierClassName(group.Key.Name)}.cs", classSource);
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

            // begin building the generated source
            StringBuilder source = new StringBuilder();
            source.AppendLineIndented(0, $"namespace {namespaceName}");
            source.AppendLineIndented(0, "{");
            source.AppendLineIndented(1, $"public class {GetNotifierClassName(classSymbol.Name)} : {notifySymbol.ToDisplayString()}");
            source.AppendLineIndented(1, "{");

            // if the class doesn't implement INotifyPropertyChanged already, add it
            if (!classSymbol.Interfaces.Contains(notifySymbol))
            {
                source.AppendLineIndented(2, "public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
            }

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
            string fieldName = propertySymbol.Name;
            ITypeSymbol fieldType = propertySymbol.Type;

            // get the ViewModel attribute from the field, and any associated data
            AttributeData attributeData = propertySymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            TypedConstant overridenNameOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

            string propertyName = ChooseName();
            if (propertyName.Length == 0 || propertyName == fieldName)
            {
                //TODO: issue a diagnostic that we can't process this field
                return;
            }

            source.AppendLineIndented(2, "");
            source.AppendLineIndented(2, $"private {fieldType} {fieldName};");
            source.AppendLineIndented(2, $"/// <inheritdoc cref=\"{recordName}.{fieldName}\" />");
            source.AppendLineIndented(2, $"public {fieldType} {propertyName}");
            source.AppendLineIndented(2, "{");
            source.AppendLineIndented(3, $"get => this.{fieldName};");
            source.AppendLineIndented(3, "init //hello");
            source.AppendLineIndented(3, "{");
            source.AppendLineIndented(4, $"this.{fieldName} = value;");
            source.AppendLineIndented(4, $"this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertyName})));");
            source.AppendLineIndented(3, "}");
            source.AppendLineIndented(2, "}");

            string ChooseName()
            {
                if (!overridenNameOpt.IsNull)
                {
                    return overridenNameOpt.Value.ToString();
                }

                fieldName = fieldName.TrimStart('_');
                if (fieldName.Length == 0)
                    return string.Empty;

                if (fieldName.Length == 1)
                    return fieldName.ToUpper();

                return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
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

