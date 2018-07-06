using System;
using System.Globalization;
using Xamarin.Forms;

namespace Yol.Punla.Converters
{
    public class DateTextToElapsedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = "";

            if (value != null)
            {
                DateTime dateTimeResult = DateTime.MinValue;

                //chito. to be used soon...
                //string[] formats = {
                //    "M/d/yyyy h:mm:ss tt",
                //    "M/d/yyyy h:mm tt",
                //    "MM/dd/yyyy hh:mm:ss",
                //    "M/d/yyyy h:mm:ss",
                //    "M/d/yyyy hh:mm tt",
                //    "M/d/yyyy hh tt",
                //    "M/d/yyyy h:mm",
                //    "M/d/yyyy h:mm",
                //    "MM/dd/yyyy hh:mm",
                //    "M/dd/yyyy hh:mm",
                //    "yyyy-MM-ddThh:mm:ss.fffZ",
                //    "yyyy-MM-dd",
                //    "yyyy-MM-ddThh:mm:ss.fffZ"
                //};

                bool isConversionSuccess = DateTime.TryParse(value.ToString(), out dateTimeResult);

                if (!isConversionSuccess)
                    return "Just now";

                var datetime = DateTime.SpecifyKind(dateTimeResult, DateTimeKind.Utc).ToLocalTime();

                if (DateTime.Now.Subtract(datetime).Days >= 7)
                    return returnValue = $"{datetime.ToString("MMM d")}";

                if (DateTime.Now.Subtract(datetime).Days > 1)
                    return returnValue = $"{DateTime.Now.Subtract(datetime).Days} days ago";

                if (DateTime.Now.Subtract(datetime).Days == 1)
                    return returnValue = $"Yesterday at {datetime.ToString("h:mm tt")}";

                if (DateTime.Now.Subtract(datetime).Hours > 1)
                    return returnValue = $"{DateTime.Now.Subtract(datetime).Hours} hours ago";

                if (DateTime.Now.Subtract(datetime).Hours == 1)
                    return returnValue = $"an hour ago";

                if (DateTime.Now.Subtract(datetime).Minutes > 1)
                    return returnValue = $"{DateTime.Now.Subtract(datetime).Minutes} minutes ago";

                if (DateTime.Now.Subtract(datetime).Minutes == 1)
                    return returnValue = "a minute ago";

                if (DateTime.Now.Subtract(datetime).Minutes < 1)
                    returnValue = "Just now";
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
