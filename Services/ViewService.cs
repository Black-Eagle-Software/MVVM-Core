using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BES.MVVM.Core.Messages;

namespace BES.MVVM.Core.Services {
    //taken from http://astoundingprogramming.wordpress.com/2012/02/23/mvvm-light-is-cool-viewmodellocator-sucks/
    public class ViewService : IViewService {
        readonly Dictionary<Type, Type> _viewMap;
        readonly List<Window> _openedWindows;

        public ViewService() {
            this._viewMap = new Dictionary<Type, Type>();
            this._openedWindows = new List<Window>();
        }

        public void RegisterView( Type windowType, Type viewModelType ) {
            lock ( this._viewMap ) {
                if ( this._viewMap.ContainsKey( viewModelType ) )
                    throw new ArgumentException( "ViewModel already registered" );
                this._viewMap[viewModelType] = windowType;
            }
        }

        [DebuggerStepThrough]
        public void OpenWindow( ViewModelBase viewModel ) {
            // Create window for that view tabModel.
            var window = this.CreateWindow( viewModel );
            
            // Open the window.
            window.Show();
        }

        [DebuggerStepThrough]
        public bool? OpenDialog( ViewModelBase viewModel ) {
            // Create window for that view tabModel.
            var window = this.CreateWindow( viewModel );

            // Open the window and return the result.
            return window.ShowDialog();
        }

        Window CreateWindow( ViewModelBase viewModel ) {
            Type windowType;
            lock ( this._viewMap ) {
                if ( !this._viewMap.ContainsKey( viewModel.GetType() ) )
                    throw new ArgumentException( "viewModel not registered" );
                windowType = this._viewMap[viewModel.GetType()];
            }

            var window = ( Window )Activator.CreateInstance( windowType );
            window.DataContext = viewModel;
            window.Closed += this.OnClosed;

            lock ( this._openedWindows ) {
                // Last window opened is considered the 'owner' of the window.
                // May not be 100% correct in some situations but it is more
                // then good enough for handling dialog windows
                if ( this._openedWindows.Count > 0 ) {
                    var lastOpened = this._openedWindows[this._openedWindows.Count - 1];

                    if ( window != lastOpened )
                        window.Owner = lastOpened;
                }

                this._openedWindows.Add( window );
            }

            // Listen for the close event
            Messenger.Default.Register<RequestCloseMessage>( window, viewModel, this.OnRequestClose );

            return window;
        }

        void OnRequestClose( RequestCloseMessage message ) {
            var window = this._openedWindows.SingleOrDefault( w => w.DataContext == message.ViewModel );
            if ( window == null ) return;
            Messenger.Default.Unregister<RequestCloseMessage>( window, message.ViewModel, this.OnRequestClose );
            if ( message.DialogResult != null ) {
                // Trying to set the dialog result of the non-modal window will
                // result in an InvalidOperationException
                window.DialogResult = message.DialogResult;
            }
            window.Close();
        }

        void OnClosed( object sender, EventArgs e ) {
            var window = sender as Window;
            window.Closed -= this.OnClosed;

            lock ( this._openedWindows ) {
                this._openedWindows.Remove( window );
            }
        }
    }
}
