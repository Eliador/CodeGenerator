using System;

namespace CodeGenerator.CSharp.Common
{
    public class CodeGeneratorFailException : Exception
    {
        private string _objectDefinitionSource;

        public CodeGeneratorFailException(string message, string objectDefinitionSource = null) 
            : base(message)
        {
            _objectDefinitionSource = objectDefinitionSource;
        }

        public string ObjectDefinitionSource => _objectDefinitionSource;
    }
}
