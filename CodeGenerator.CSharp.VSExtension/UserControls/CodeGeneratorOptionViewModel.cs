using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.CSharp.VSExtension.UserControls
{
    internal class CodeGeneratorOptionViewModel : INotifyPropertyChanged
    {
        private readonly CodeGeneratorOptionPageGrid _optionsModel;

        public CodeGeneratorOptionViewModel(CodeGeneratorOptionPageGrid optionsModel)
        {
            _optionsModel = optionsModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string NameSpace
        {
            get { return _optionsModel.NameSpace; }
            set
            {
                _optionsModel.NameSpace = value;
                NotifyPropertyChanged(nameof(NameSpace));
            }
        }

        public bool IsSealed
        {
            get { return _optionsModel.IsSealed; }
            set
            {
                _optionsModel.IsSealed = value;
                NotifyPropertyChanged(nameof(IsSealed));
            }
        }

        public bool RedefineNameSpace
        {
            get { return _optionsModel.RedefineNameSpace; }
            set
            {
                _optionsModel.RedefineNameSpace = value;
                NotifyPropertyChanged(nameof(RedefineNameSpace));
            }
        }

        public bool InSingleFile
        {
            get { return _optionsModel.InSingleFile; }
            set
            {
                _optionsModel.InSingleFile = value;
                NotifyPropertyChanged(nameof(InSingleFile));
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        
    }
}
