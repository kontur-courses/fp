﻿using ResultOf;
using System.Drawing.Imaging;

namespace TagCloud2.Image
{
    public class JpegImageFormatter : IImageFormatter
    {
        public Result<ImageCodecInfo> Codec => ImageFormatterHelper.GetEncoderInfo("image/jpeg");

        public EncoderParameters Parameters => GetParameters();

        private EncoderParameters GetParameters()
        {
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            return myEncoderParameters;
        }
    }
}
