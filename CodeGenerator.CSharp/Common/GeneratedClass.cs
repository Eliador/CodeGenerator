using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.CSharp.Common
{
    public class GeneratedItem
    {
        public string Name { get; set; }

        public SyntaxNode SyntaxNode { get; set; }

        public IEnumerable<string> Usings { get; set; }
    }
}
