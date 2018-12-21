using System;

namespace TagCloud.Layouter
{
    public class Size
    {
        public Size(double width, double height)
        {
            if (width < 0)
                throw new Exception("Bad size scheme: width must be a positive number, but given: " + width);
            if (height < 0)
                throw new Exception("Bad size scheme: height must be a positive number, but given: " + height);
            Width = width;
            Height = height;
        }

        public double Width { get; }
        public double Height { get; }
        public double Square => Width * Height;
    }
}