﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Yol.Punla.Converters
{
    public class BooleanInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    }
}
