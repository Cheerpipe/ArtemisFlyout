using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ArtemisFlyout.Converters
{
    // ReSharper disable once UnusedMember.Global
    public class SummatoryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && targetType == typeof(double) &&
                double.TryParse((string)parameter,
                    NumberStyles.Integer, culture, out var parameterValue))
            {
                return (double)value + parameterValue;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
