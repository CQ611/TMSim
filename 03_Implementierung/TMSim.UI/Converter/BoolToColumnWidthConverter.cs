using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TMSim.UI
{
    public class BoolToColumnWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value == false)
            {
                return new GridLength(0); //Unsichtbar
            }

            return new GridLength(1, GridUnitType.Star); //Sichtbar            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new ArgumentException();
        }
    }
}
