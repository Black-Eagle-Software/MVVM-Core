using System;
using GalaSoft.MvvmLight;

namespace BES.MVVM.Core.Services {
    //taken from http://astoundingprogramming.wordpress.com/2012/02/23/mvvm-light-is-cool-viewmodellocator-sucks/
    public interface IViewService : IServiceManagerService {
        void RegisterView( Type windowType, Type viewModelType );

        void OpenWindow( ViewModelBase viewModel );
        bool? OpenDialog( ViewModelBase viewModel );
    }
}
