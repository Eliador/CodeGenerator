using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeGenerator.CSharp.VSExtension.UserControls
{
    [Guid("32550DC6-1F85-42A5-A827-A665F2B7C8D0")]
    internal class CodeGeneratorOptionPageGrid : DialogPage
    {
        private CodeGeneratorOptionsControl _page;

        [Category("NameSpace")]
        [DisplayName("Redefine Name Space")]
        [Description("Redefine Name Space")]
        public bool RedefineNameSpace { get; set; }

        [Category("NameSpace")]
        [DisplayName("Name Space")]
        [Description("Name Space")]
        public string NameSpace { get; set; }

        [Category("ClassDefenition")]
        [DisplayName("Make classes as sealed")]
        [Description("Make classes as sealed")]
        public bool IsSealed { get; set; }

        [Category("GenerationResult")]
        [DisplayName("Put all classes in one file")]
        [Description("Put all classes in one file")]
        public bool InSingleFile { get; set; }

        protected override IWin32Window Window
        {
            get
            {
                if (_page == null)
                {
                    _page = new CodeGeneratorOptionsControl(this);
                }

                return _page;
            }
        }
    }
}
