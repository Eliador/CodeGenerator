using CodeGenerator.CSharp.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
// This is resolve a problem with not suppoting of C# Language Exception
using FormattingOptions = Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions;

namespace CodeGenerator.CSharp
{
    public class ByJsonSchemaGenerator
    {
        private SyntaxGenerator _generator;        

        public ByJsonSchemaGenerator()
        {
            var workspace = new AdhocWorkspace();
            _generator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);            
        }

        public GenerationResult GenerateSingleItem(GenerationProperties properties)
        {
            var result = GenerateInternal(properties);

            var nodes = result.Classes.Select(x => x.SyntaxNode).Union(result.Enums.Select(x => x.SyntaxNode));
            var usings = result.Classes.SelectMany(x => x.Usings).Distinct();

            var code = GenerateSourceCodeString(usings, properties.NameSpace, nodes.ToArray());

            return new GenerationResult
            {
                Code = code,
                FileName = "GeneratedClasses"
            };
        }

        public IEnumerable<GenerationResult> GenerateSeparateItems(GenerationProperties properties)
        {
            var result = GenerateInternal(properties);

            var classes = result.Classes.Select(x => new GenerationResult
            {
                Code = GenerateSourceCodeString(x.Usings, properties.NameSpace, x.SyntaxNode),
                FileName = x.Name                
            });

            var enums = result.Enums.Select(x => new GenerationResult
            {
                Code = GenerateSourceCodeString(new List<string>(), properties.NameSpace, x.SyntaxNode),
                FileName = x.Name
            });

            return classes.Union(enums);
        }

        private (IEnumerable<GeneratedItem> Enums, IEnumerable<GeneratedItem> Classes) GenerateInternal(GenerationProperties properties)
        {
            if (string.IsNullOrEmpty(properties.NameSpace))
            {
                throw new CodeGeneratorFailException(@"'NameSpace' should be determined.");
            }

            var schema = JSchema.Parse(properties.JsonSchema);

            var context = new GenerationContext(properties);
            BuildClassExpression(schema, context);

            context.GeneratedClasses.Reverse();

            var classes = DistinctDeclarations(context.GeneratedClasses);
            var enums = DistinctDeclarations(context.GeneratedEnums);

            return (enums, classes);
        }

        private IEnumerable<GeneratedItem> DistinctDeclarations(List<GeneratedItem> items)
        {
            var groupsByName = items.GroupBy(x => x.Name);

            foreach (var namedGroup in groupsByName)
            {
                foreach (var item in namedGroup)
                {
                    if (namedGroup.Any(x => !x.SyntaxNode.IsEquivalentTo(item.SyntaxNode)))
                    {
                        throw new CodeGeneratorFailException($"Several items have a same Name [{item.Name}].");
                    }                    
                }

                yield return namedGroup.First();
            }
        }

        private string GenerateSourceCodeString(IEnumerable<string> usings, string @namespace, params SyntaxNode[] classDeclarations)
        {
            var declarations = usings.Select(x => _generator.NamespaceImportDeclaration(x)).ToList();
            declarations.Add(_generator.NamespaceDeclaration(@namespace, classDeclarations));

            return _generator.CompilationUnit(declarations)
                .NormalizeWhitespace()
                .ToFullString();
        }

        private TypeSyntax BuildClassExpression(JSchema schema, GenerationContext context)
        {
            var properties = new List<SyntaxNode>();
            var usings = new List<string>();
            
            foreach (var property in schema.Properties)
            {
                var propertyName = ToCamelCase(property.Key, property.Value);
                properties.Add(BuildPropertyExpression(propertyName, property.Value, context));

                if (property.Value.Type == JSchemaType.Array)
                {
                    usings.Add(typeof(IEnumerable<object>).Namespace);
                }
            }

            var modifiers = DeclarationModifiers.None;
            if (context.IsSealed)
            {
                modifiers |= DeclarationModifiers.Sealed;
            }

            var className = ToCamelCase(schema.Title, schema);
            var classDeclaration = _generator.ClassDeclaration(className, null, Accessibility.Public, modifiers, null, null, properties);

            var result = new GeneratedItem
            {
                SyntaxNode = classDeclaration,
                Usings = usings,
                Name = className
            };

            context.GeneratedClasses.Add(result);

            return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(className));
        }        

        private SyntaxNode BuildPropertyExpression(string propertyName, JSchema schema, GenerationContext context)
        {
            var propertyType = BuildTypeExpression(schema, context);

            return _generator.AutoPropertyDeclaration(
                propertyName,
                schema.Required.Contains(propertyName) ? _generator.NullableTypeExpression(propertyType) : propertyType,
                Accessibility.Public);
        }

        private SyntaxNode BuildEnumExpression(JSchema schema, GenerationContext context)
        {
            var name = ToCamelCase(schema.Title, schema);
            var members = schema.Enum.Select(x => x.ToString());

            var membersSyntaxList = members.Select(x => SyntaxFactory.EnumMemberDeclaration(x));
            var enumDeclaration = _generator.EnumDeclaration(name, Accessibility.Public, members: membersSyntaxList);
            context.GeneratedEnums.Add(new GeneratedItem
            {
                SyntaxNode = enumDeclaration,
                Name = name
            });
            
            return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(name));
        }

        private SyntaxNode BuildTypeExpression(JSchema schema, GenerationContext context)
        {
            if (schema.Enum != null && schema.Enum.Any())
            {
                return BuildEnumExpression(schema, context);
            }

            switch (schema.Type)
            {
                case JSchemaType.Integer:
                    return _generator.TypeExpression(SpecialType.System_Int32);
                case JSchemaType.Boolean:
                    return _generator.TypeExpression(SpecialType.System_Boolean);
                case JSchemaType.Number:
                    return _generator.TypeExpression(SpecialType.System_Double);
                case JSchemaType.String:
                    return _generator.TypeExpression(SpecialType.System_String);
                case JSchemaType.Array:
                    return BuildCollectionExpression(schema, context);
                case JSchemaType.Object:
                    return BuildClassExpression(schema, context);
                default:
                    throw new NotImplementedException();
            }
        }

        private SyntaxNode BuildCollectionExpression(JSchema schema, GenerationContext context)
        {
            if (schema.Items.Count != 1 || schema.Items.Single().OneOf.Count != 1)
            {
                throw new CodeGeneratorFailException("Item type definition in the array object should be only one.", schema.ToString());
            }

            var typeDefenitionSchema = schema.Items.Single().OneOf.Single();
            var collectionType = BuildTypeExpression(typeDefenitionSchema, context);

            return _generator.IEnumerableTypeExpression(collectionType);
        }

        private string ToCamelCase(string name, JSchema schema)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CodeGeneratorFailException("Class name should be determined ('title' of object type in schema is required)", schema.ToString());
            }

            if (!char.IsLetter(name[0]))
            {
                throw new CodeGeneratorFailException($"Class name [{name}] should started from letter", schema.ToString());
            }

            if (name.Length > 1)
                return char.ToUpper(name[0]) + name.Substring(1);

            return name.ToUpper();
        }

        private class GenerationContext
        {
            public GenerationContext(GenerationProperties properties)
            {
                IsSealed = properties.IsSealed;
            }

            public List<GeneratedItem> GeneratedClasses { get; } = new List<GeneratedItem>();

            public List<GeneratedItem> GeneratedEnums { get; } = new List<GeneratedItem>();

            public bool IsSealed { get; }
        }
    }
}
