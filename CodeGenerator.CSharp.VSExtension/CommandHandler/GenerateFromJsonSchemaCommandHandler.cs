using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using CodeGenerator.CSharp.Common;
using CodeGenerator.CSharp.VSExtension.UserControls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodeGenerator.CSharp.VSExtension.CommandHandler
{
    internal sealed class GenerateFromJsonSchemaCommandHandler
    {
        public const int CommandId = 0x0100;        
        private const string FileExtension = ".json";
        private const string SchemaIdentifier = ".schema";

        public static readonly Guid CommandSet = new Guid("28831623-6a51-42ce-910c-4e6261597c74");

        private readonly IServiceProvider _serviceProvider;
        private readonly CodeGeneratorOptionPageGrid _packageOptions;
        private readonly ByJsonSchemaGenerator _generator;

        private GenerateFromJsonSchemaCommandHandler(Package package)
        {
            _serviceProvider = package ?? throw new ArgumentNullException("package");
            _packageOptions = package.GetDialogPage(typeof(CodeGeneratorOptionPageGrid)) as CodeGeneratorOptionPageGrid;
            _generator = new ByJsonSchemaGenerator();

            OleMenuCommandService commandService = _serviceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(InvokeEventHandler, menuCommandID);
                commandService.AddCommand(menuItem);
                menuItem.BeforeQueryStatus += beforeQueryStatusEventHandler;
            }
        }

        public static GenerateFromJsonSchemaCommandHandler Instance
        {
            get;
            private set;
        }

        public static void Initialize(Package package)
        {
            Instance = new GenerateFromJsonSchemaCommandHandler(package);
        }

        private void InvokeEventHandler(object sender, EventArgs e)
        {
            var dte = (EnvDTE.DTE)_serviceProvider.GetService(typeof(EnvDTE.DTE));
            var selectedItem = dte.SelectedItems.Item(1);
            var project = selectedItem.ProjectItem.ContainingProject;

            var pathProjectParts = project.FullName.Split('\\').Except(new[] { project.Name });
            var fullSchemaName = (string)selectedItem.ProjectItem.Properties.Item("FullPath").Value;
            var schemaPath = Path.GetDirectoryName(fullSchemaName);
            var nameSpace = String.Join(".", schemaPath
                .Split('\\')
                .Where(x => !pathProjectParts.Contains(x) && x != string.Empty));

            var schema = File.ReadAllText(fullSchemaName);
            var schemaName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(selectedItem.Name));

            var generationResult = GenerateCode(schema, nameSpace, schemaName);

            foreach (var generationItem in generationResult)
            {
                var newFileName = $"{schemaPath}\\{generationItem.FileName}.cs";
                File.WriteAllText(newFileName, generationItem.Code);
                project.ProjectItems.AddFromFile(newFileName);
            }
        }

        private void beforeQueryStatusEventHandler(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            var dte = _serviceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            if (dte.SelectedItems.Count != 1)
            {
                command.Visible = false;
                return;
            }

            var itemName = dte.SelectedItems.Item(1).Name;
            var fileExtension = Path.GetExtension(itemName);
            var schemaIdentifier = Path.GetExtension(Path.GetFileNameWithoutExtension(itemName));

            if (fileExtension != FileExtension || schemaIdentifier != SchemaIdentifier)
            {
                command.Visible = false;
                return;
            }

            command.Visible = true;
        }

        private IEnumerable<GenerationResult> GenerateCode(string schema, string nameSpace, string schemaName)
        {
            if (_packageOptions.RedefineNameSpace && string.IsNullOrWhiteSpace(_packageOptions.NameSpace))
            {
                VsShellUtilities.ShowMessageBox(
                    _serviceProvider,
                    "Redefined namespace should not be Empty",
                    "Generation Failed!",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return Enumerable.Empty<GenerationResult>();
            }

            var properties = new GenerationProperties
            {
                JsonSchema = schema,
                IsSealed = _packageOptions.IsSealed,
                NameSpace = _packageOptions.RedefineNameSpace ? _packageOptions.NameSpace : nameSpace
            };

            try
            {
                if (_packageOptions.InSingleFile)
                {
                    var result = _generator.GenerateSingleItem(properties);
                    result.FileName = schemaName;

                    return new[] { result };
                }
                else
                {
                    return _generator.GenerateSeparateItems(properties);
                }
            }
            catch (CodeGeneratorFailException exception)
            {
                VsShellUtilities.ShowMessageBox(
                    _serviceProvider,
                    string.IsNullOrEmpty(exception.ObjectDefinitionSource) ? string.Empty : $"Root node of achema:\n{exception.ObjectDefinitionSource}",
                    $"Error: {exception.Message}",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return Enumerable.Empty<GenerationResult>();
            }
            catch (Exception exception)
            {
                VsShellUtilities.ShowMessageBox(
                    _serviceProvider,
                    exception.Message,
                    $"Unhandled Exception!",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return Enumerable.Empty<GenerationResult>();
            }
        }
    }
}
