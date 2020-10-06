namespace CodeGenerator.CSharp
{
    public class GenerationProperties
    {
        public string NameSpace { get; set; } = "MyNameSpace";

        public string JsonSchema { get; set; }

        public bool IsSealed { get; set; }
    }
}
