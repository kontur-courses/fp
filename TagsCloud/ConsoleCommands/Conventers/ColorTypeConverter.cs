using System.ComponentModel;
using System.Drawing;
using ArgumentException = System.ArgumentException;

namespace TagsCloud.ConsoleCommands.Conventers;

public class ColorTypeConverter:TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(Color) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
        object value)
    {
        if (value is string stringValue)
        {
            if (!Color.FromName(stringValue).IsKnownColor)
            {
                throw new ArgumentException();
            }

            return Color.FromName(stringValue);
        }

        throw new ArgumentException();
    }
}