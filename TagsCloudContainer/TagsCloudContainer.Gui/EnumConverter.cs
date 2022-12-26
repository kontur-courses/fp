using System;
using System.Globalization;
using System.Windows.Data;

namespace TagsCloudContainer.Gui;

public class EnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var type = value.GetType();
        return !type.IsAssignableTo(typeof(Enum)) ? string.Empty : Enum.GetName(type, value)!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.TryParse(targetType, (string)value, out var result)
            ? result
            : Enum.GetValues(targetType).GetValue(0)!;
    }
}