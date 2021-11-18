using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Material.Icons;

namespace ArtemisFlyout.Converters
{
    public class MaterialIconEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && targetType == typeof(MaterialIconKind)
                ? Enum.TryParse(typeof(MaterialIconKind), value.ToString(), out object iconKind) ? iconKind :
                MaterialIconKind.HelpCircle
                : AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
