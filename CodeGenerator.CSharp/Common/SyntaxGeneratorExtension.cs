using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.CSharp.Common
{
    public static class SyntaxGeneratorExtension
    {
        public static SyntaxNode GenericTypeExpression(this SyntaxGenerator generator, string typeName, params SyntaxNode[] argumentsTypes)
        {
            var separatedSyntaxList = SyntaxFactory.SeparatedList(argumentsTypes);
            
            return SyntaxFactory.GenericName(
                SyntaxFactory.Identifier(typeName),
                SyntaxFactory.TypeArgumentList(separatedSyntaxList));
        }

        public static SyntaxNode IEnumerableTypeExpression(this SyntaxGenerator generator, SyntaxNode argumentType)
        {
            var typeNmae = nameof(IEnumerable<object>);

            return generator.GenericTypeExpression(typeNmae, argumentType);
        }

        public static SyntaxNode AutoPropertyDeclaration(this SyntaxGenerator generator, string propertyName, SyntaxNode type, Accessibility accessibility, DeclarationModifiers modifiers = default(DeclarationModifiers))
        {
            var getAccessorDeclaration = SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            var setAccessorDeclaration = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            var propertyDeclaraton = ((PropertyDeclarationSyntax)generator.PropertyDeclaration(propertyName, type, Accessibility.Public))
                .WithAccessorList(SyntaxFactory.AccessorList())
                .AddAccessorListAccessors(getAccessorDeclaration, setAccessorDeclaration);

            return propertyDeclaraton;
        }
    }
}
