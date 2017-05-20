using System;
using System.Collections.Generic;

namespace BES.MVVM.Core.Services {
    //taken from http://astoundingprogramming.wordpress.com/2012/02/23/mvvm-light-is-cool-viewmodellocator-sucks/
    public class ServiceManager {
        static readonly ServiceManager _instance = new ServiceManager();
        static readonly Dictionary<Type, IServiceManagerService> Services = new Dictionary<Type, IServiceManagerService>();

        ServiceManager() { } // Private constructor

        public static T RegisterService<T>( IServiceManagerService service ) {
            return _instance.Register<T>( service );
        }

        public static T GetService<T>() where T : IServiceManagerService {
            return _instance.Get<T>();
        }

        public void RemoveService<T>() where T : IServiceManagerService {
            _instance.Remove<T>();
        }

        public void ClearServices() {
            _instance.Clear();
        }

        T Register<T>( IServiceManagerService service ) {
            if ( service == null )
                throw new ArgumentNullException( "service" );

            lock ( Services ) {
                if ( Services.ContainsKey( typeof( T ) ) )
                    throw new ArgumentException( "Service already registered" );
                Services[typeof( T )] = service;
            }
            return ( T )service;
        }

        T Get<T>() where T : IServiceManagerService {
            lock ( Services ) {
                if ( Services.ContainsKey( typeof( T ) ) )
                    return ( T )Services[typeof( T )];
                else
                    throw new ArgumentException( "Service not registered: " + typeof( T ) );
            }
        }

        void Remove<T>() where T : IServiceManagerService {
            lock ( Services ) {
                if ( Services.ContainsKey( typeof( T ) ) )
                    Services.Remove( typeof( T ) );
            }
        }

        void Clear() {
            lock ( Services ) {
                Services.Clear();
            }
        }
    }
}
