using System;
using System.Globalization;
using Xamarin.Forms;

namespace Yol.Punla.Converters
{
    public class BooleanToIsEnableColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Color.White;
            else
                return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Color.White;
            else
                return Color.Gray;
        }
    }
}
