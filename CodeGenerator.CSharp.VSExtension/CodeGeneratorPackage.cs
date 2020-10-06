using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CodeGenerator.CSharp.VSExtension.CommandHandler;
using CodeGenerator.CSharp.VSExtension.UserControls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodeGenerator.CSharp.VSExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideOptionPage(typeof(CodeGeneratorOptionPageGrid), "Code Generator", "From JSON schema Generator", 0, 0, true)]
    public sealed class CodeGeneratorPackage : Package
    {
        public const string PackageGuidString = "1c149e12-964b-42f2-ab0e-0c255eadacc8";

        protected override void Initialize()
        {
            GenerateFromJsonSchemaCommandHandler.Initialize(this);
            base.Initialize();
        }
    }
}
