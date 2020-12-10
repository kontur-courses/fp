using System.Drawing;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont
{
    public interface IFontConverter
    {
        Font ConvertToFont(string[] fontParameters);
    }
}