using System;
using System.IO;

namespace CodeGenerator.CSharp.StandAlone
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start generating...");

            var properties = PrepareGenerationProperties(args);

            var generator = new ByJsonSchemaGenerator();

            if (properties.InSingleFile)
            {
                var result = generator.GenerateSingleItem(properties.Properties);

                File.WriteAllText(properties.Out + $"\\{result.FileName}.cs", result.Code);
            }
            else
            {
                var results = generator.GenerateSeparateItems(properties.Properties);

                foreach (var item in results)
                {
                    File.WriteAllText(properties.Out + $"\\{item.FileName}.cs", item.Code);
                }
            }

            Console.WriteLine("Done!");
        }

        private static (GenerationProperties Properties, string Out, bool InSingleFile) PrepareGenerationProperties(string[] args)
        {
            var properties = new GenerationProperties();
            var outDir = string.Empty;
            var inSingleFile = false;
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-schema":
                        properties.JsonSchema = LoadSchema(args[++i]);
                        break;
                    case "-out":
                        outDir = args[++i];
                        break;
                    case "-sealed":
                        properties.IsSealed = true;
                        break;
                    case "-ns":
                        properties.NameSpace = args[++i];
                        break;
                    case "-sf":
                        inSingleFile = true;
                        break;
                }
            }

            return (properties, outDir, inSingleFile);
        }

        static private string LoadSchema(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"'{filePath}' is not exists.");
            }

            return File.ReadAllText(filePath);
        }
    }
}
