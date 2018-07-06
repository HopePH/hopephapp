using System;
using System.Resources;
using System.Reflection;
using System.Globalization;
using Yol.Punla.Barrack;

namespace Yol.Punla.Localized
{
    public class LocalizationService
    {
        private readonly ResourceManager resourceManager;
        private const string resourceID = "Yol.Punla.Localized.AppStrings";

        private static LocalizationService InstanceField;

        public static LocalizationService Instance
        {
            get
            {
                if (InstanceField == null)
                {
                    InstanceField = new LocalizationService();
                }
                return InstanceField;
            }
        }

        public CultureInfo MyCulture
        {
            get
            {
                string cultureCode = AppSettingsProvider.Instance.GetValue("AppCulture");

                if (String.IsNullOrEmpty(cultureCode))
                    return CultureInfo.CurrentCulture;

                return new CultureInfo(AppSettingsProvider.Instance.GetValue("AppCulture"));
            }
        }

        private LocalizationService()
        {
            this.resourceManager = new ResourceManager(resourceID, typeof(AppStrings).GetTypeInfo().Assembly);
            AppStrings.Culture = this.MyCulture;
        }

        public string Write(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            string localText = this.resourceManager.GetString(text, MyCulture);  

            if (localText == null)
            {
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", 
                    text, resourceID, MyCulture.Name), "Text");
            }

            return localText;

        }
    }
}
