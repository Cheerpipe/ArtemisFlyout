using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ArtemisFlyout.Converters
{
    public class SubtractionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = value;
            double parameterValue;

            if (value != null && targetType == typeof(double) &&
                double.TryParse((string)parameter,
                    NumberStyles.Integer, culture, out parameterValue))
            {
                return (double)value - (double)parameterValue;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
