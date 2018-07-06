using System;

namespace Yol.Punla.Utility
{
    public interface IKeyValueCacheUtility
    {
        String GetUserDefaultsKeyValue(string key, string setInitialValueWhenNull = "");
        void RemoveKeyObject(string key);
    }
}
