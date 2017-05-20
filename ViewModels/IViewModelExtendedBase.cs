using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BES.MVVM.Core.ViewModels {
    public interface IViewModelExtendedBase {
        ICommand CloseCommand { get; }

        void Close();
    }
}
