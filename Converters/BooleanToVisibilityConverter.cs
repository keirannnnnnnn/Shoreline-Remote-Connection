using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VNCClient
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public static readonly BooleanToVisibilityConverter Default = new();
        public static readonly BooleanToVisibilityConverter Inverted = new() { IsInverted = true };

        public bool IsInverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (IsInverted)
                    boolValue = !boolValue;
                
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                if (IsInverted)
                    result = !result;
                
                return result;
            }
            
            return false;
        }
    }
}