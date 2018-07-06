using System;
using System.Collections.Generic;

namespace Yol.Punla.Barrack
{

    public class AppSettingsProvider
    {
        private static AppSettingsProvider InstanceObj;

        public static AppSettingsProvider Instance
        {
            get
            {
                if (InstanceObj == null)
                    InstanceObj = new AppSettingsProvider();

                return InstanceObj;
            }
        }

        private Func<Dictionary<String, String>> _defaultSettingsDelegate;

        private Dictionary<String, String> _appSettings;

        public void SetAppSettings(Func<Dictionary<String, String>> _action)
        {
            _defaultSettingsDelegate = _action;
        }

        public string GetValue(string key)
        {
            if (_appSettings == null)
                _appSettings = _defaultSettingsDelegate();

            var theSettings = _appSettings;
            string _value = String.Empty;

            if (!theSettings.TryGetValue(key, out _value))
                return String.Empty;

            return _value;
        }
    }
}
