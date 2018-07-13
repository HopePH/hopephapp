using Java.Util;
using System;

namespace Yol.Punla.Droid.Utility
{
    public class ExtendedDataHolder
    {
        private static readonly ExtendedDataHolder _instance = new ExtendedDataHolder();

        private readonly HashMap extras = new HashMap();

        private ExtendedDataHolder()
        {
            
        }

        public static ExtendedDataHolder Instance
        {
            get
            {
                if (_instance == null)
                    throw new ArgumentNullException("The Unity Container was not initialized at AppUnityContainer!");

                return _instance;
            }
        }

        public void PutExtra(String name, Java.Lang.Object objValue)
        {
            extras.Put(name, objValue);
        }

        public Object GetExtra(String name)
        {
            return extras.Get(name);
        }

        public bool HasExtra(String name)
        {
            return extras.ContainsKey(name);
        }

        public void Clear()
        {
            extras.Clear();
        }
    }
}