using System;
using System.Windows.Forms;

namespace CodeGenerator.CSharp.VSExtension.UserControls
{
    internal partial class CodeGeneratorOptionsControl : UserControl
    {
        private readonly BindingSource _bindingSource;

        public CodeGeneratorOptionsControl(CodeGeneratorOptionPageGrid options)
        {
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = typeof(CodeGeneratorOptionViewModel);
            var dataSource = new CodeGeneratorOptionViewModel(options);
            _bindingSource.Add(dataSource);

            InitializeComponent();

            BuildBinding(NameSpaceTextBox, nameof(NameSpaceTextBox.Text), _bindingSource, nameof(dataSource.NameSpace));
            BuildBinding(NameSpaceTextBox, nameof(NameSpaceTextBox.Enabled), _bindingSource, nameof(dataSource.RedefineNameSpace), (val) => !((bool)val));
            BuildBinding(RedefineNameSpaceCheckBox, nameof(RedefineNameSpaceCheckBox.Checked), _bindingSource, nameof(dataSource.RedefineNameSpace));
            BuildBinding(IsSealedCheckBox, nameof(IsSealedCheckBox.Checked), _bindingSource, nameof(dataSource.IsSealed));
            BuildBinding(InSingleFileCheckBox, nameof(InSingleFileCheckBox.Checked), _bindingSource, nameof(dataSource.InSingleFile));
        }

        private void BuildBinding(Control control, string controlPropertyName, BindingSource bindingSource, string sourcePropertyName, Func<object, object> converter = null)
        {
            var binding = new Binding(controlPropertyName, bindingSource, sourcePropertyName, true, DataSourceUpdateMode.OnPropertyChanged);
            if (converter != null)
            {
                binding.Parse += (object sender, ConvertEventArgs e) =>
                {
                    e.Value = converter(e.Value);
                };
            }

            control.DataBindings.Add(binding);
        }
    }
}
