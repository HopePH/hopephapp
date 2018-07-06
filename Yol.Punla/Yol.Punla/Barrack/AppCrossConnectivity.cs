using Plugin.Connectivity.Abstractions;
using System;

namespace Yol.Punla.Barrack
{
    public static class AppCrossConnectivity
    {
        static IConnectivity InstanceObj;

        public static void Init(IConnectivity storeObj)
        {
            InstanceObj = storeObj;
        }

        public static IConnectivity Instance
        {
            get
            {
                if (InstanceObj == null)
                    throw new ArgumentNullException("The Unity Container was not initialized at AppUnityContainer!");

                return InstanceObj;
            }
        }
    }
}
