using Prism.Services;
using System;
using System.Diagnostics;
using System.Linq;
using Unity;

namespace Yol.Punla.Barrack
{
    public static class AppUnityContainer
    {
        static IUnityContainer InstanceObj;

        public static void Init(IUnityContainer storeObj)
        {
            InstanceObj = storeObj;
        }

        public static IUnityContainer Instance
        {
            get
            {
                if (InstanceObj == null)
                    throw new ArgumentNullException("The Unity Container was not initialized at AppUnityContainer!");

                return InstanceObj;
            }
        }

        public static IDependencyService InstanceDependencyService
        {
            get
            {
                if (InstanceObj == null)
                    throw new ArgumentNullException("The Unity Container was not initialized at AppUnityContainer!");

                return InstanceObj.Resolve<IDependencyService>();
            }
        }

        public static void RemoveRegistration(Type registeredType)
        {
            if (InstanceObj == null)
                throw new ArgumentNullException("The Unity Container was not initialized at AppUnityContainer!");

            var registrations = InstanceObj.Registrations.Where(x => x.RegisteredType == registeredType);

            foreach (var r in registrations)
            {
                try
                {
                    r.LifetimeManager.RemoveValue();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(string.Format("HAIYANRBF An error was in AppUnityContainer.RemoveRegistration with message {0}", exception.Message));

                    if (exception.InnerException != null)
                        Debug.WriteLine(string.Format("HAIYANRBF An error was in AppUnityContainer.RemoveRegistration with message {0}", exception.InnerException.Message));
                }
            }
        }
    }
}
