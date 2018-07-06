using System;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{

    [DefaultModule]
    public static class SettingsRepository
    {

        const string LogonDate = "LogonDate";
        const string LogonStatus = "LogonStatus";
        const string LogonUser = "LogonUser";
        const string LogonURL = "LogonURL";

        public static bool SaveActivationResult(DateTime dateActivated, string logonStatus, string logonURL, string logonUser)
        {
            Application.Current.Properties[LogonDate] = dateActivated;
            Application.Current.Properties[LogonStatus] = logonStatus;
            Application.Current.Properties[LogonURL] = logonURL;
            Application.Current.Properties[LogonUser] = logonUser;

            return true;
        }

        public static Entity.Settings GetSettings()
        {
            var setting = new Entity.Settings();

            if (Application.Current.Properties.ContainsKey(LogonDate))
                setting.LogonDate = (DateTime)Application.Current.Properties[LogonDate];
            if (Application.Current.Properties.ContainsKey(LogonStatus))
                setting.LogonStatus = Application.Current.Properties[LogonStatus] as string;
            if (Application.Current.Properties.ContainsKey(LogonURL))
                setting.LogonURL = Application.Current.Properties[LogonURL] as string;
            if (Application.Current.Properties.ContainsKey(LogonUser))
                setting.LogonUser = Application.Current.Properties[LogonUser] as string;

            return setting;
        }

        public static void Clear()
        {
            Application.Current.Properties.Clear();
        }
    }
}
