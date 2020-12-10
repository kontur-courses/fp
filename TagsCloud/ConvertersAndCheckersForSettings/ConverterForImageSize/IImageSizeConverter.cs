using System.Drawing;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize
{
    public interface IImageSizeConverter
    {
        Size ConvertToSize(int[] sizeParameters);
    }
}