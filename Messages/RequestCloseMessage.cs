using GalaSoft.MvvmLight;

namespace BES.MVVM.Core.Messages {
    //taken from http://astoundingprogramming.wordpress.com/2012/02/23/mvvm-light-is-cool-viewmodellocator-sucks/
    public class RequestCloseMessage {
        public RequestCloseMessage( ViewModelBase viewModel, bool? dialogResult = null ) {
            this.ViewModel = viewModel;
            this.DialogResult = dialogResult;
        }

        public ViewModelBase ViewModel { get; set; }
        public bool? DialogResult { get; set; }
    }
}
