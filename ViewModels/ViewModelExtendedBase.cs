using BES.MVVM.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BES.MVVM.Core.ViewModels {
    public class ViewModelExtendedBase:ViewModelBase, IViewModelExtendedBase {
        public event EventHandler RequestClose;

        public ICommand CloseCommand {
            get {
                return new RelayCommand(() => {
                    if (this.CloseCommand == null) return;
                    this.MessengerInstance.Send(new LogMessage(LogType.DEBUG,
                        $"[ViewModelExtendedBase.CloseCommand] Closing view: {this}"));
                    this.RequestClose(this, EventArgs.Empty);
                });
            }
        }

        //taken from http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "") {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            if (propertyName != null) base.RaisePropertyChanged(propertyName);
            return true;
        }

        public void Close() {
            this.MessengerInstance.Send(new LogMessage(LogType.DEBUG,
                $"[ViewModelExtendedBase.CloseCommand] Closing view: {this}"));
            var onRequestClose = this.RequestClose;
            onRequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
