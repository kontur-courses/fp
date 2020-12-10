using System;
using System.Drawing;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize
{
    public class ImageSizeConverter : IImageSizeConverter
    {
        public Size ConvertToSize(int[] sizeParameters)
        {
            if (sizeParameters.Length != 2)
                throw new Exception("Invalid number of size parameters");
            return new Size(sizeParameters[0], sizeParameters[1]);
        }
    }
}