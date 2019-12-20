using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer
{
    public class DefaultWordsToSizesConverter : IWordsToSizesConverter
    {
        public Size LayoutSize { get; set; }
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }
        private Graphics graphics;

        public DefaultWordsToSizesConverter(Size layoutSize, int maxHeight = 0, int maxWidth = 0)
        {
            LayoutSize = layoutSize;
            if (maxHeight == 0)
                MaxHeight = layoutSize.Height;
            if (maxWidth == 0)
                MaxWidth = layoutSize.Width;
            if (maxHeight != 0 && maxWidth != 0)
            {
                MaxHeight = maxHeight;
                MaxWidth = maxWidth;
            }

            var bitmap = new Bitmap(layoutSize.Width, layoutSize.Height);
            Graphics g = Graphics.FromImage(bitmap);
            graphics = g;
        }

        private Size GetSizeOf(string word, Dictionary<string, int> dictionary)
        {
            var count = dictionary.Select(x => x.Value).Sum();
            var sizeFD = graphics.MeasureString(word, new Font("Tahoma", 500));
            var widthFD = sizeFD.Width * ((double) dictionary[word] / count);
            var heightFD = sizeFD.Height * ((double) dictionary[word] / count);
            return new Size(Math.Min((int) widthFD, MaxWidth),
                Math.Min((int) heightFD, MaxHeight));
        }
        
        private double GetSquare(IEnumerable<Size> sizes)
        {
            var result = 0.0;
            foreach (var rect in sizes)
            {
                result += rect.Height * rect.Width;
            }

            return result;
        }

        private IEnumerable<(string, Size)> FormSizes(Dictionary<string, int> dict)
        {
            var res = new List<(string, Size)>();
            foreach (var key in dict.Keys)
            {
                var tup = (key, GetSizeOf(key, dict));
                res.Add(tup);
            }

            return res;
        }

        private double GetCoefficientOfLayoutSquareTo(double actualSquare)
        {
            double bitmapSquare = (LayoutSize.Height - LayoutSize.Height / 2.5) * (LayoutSize.Width - LayoutSize.Width / 2.5);
            var coeff = Math.Sqrt(bitmapSquare / actualSquare);
            return coeff;
        }
        
        public static Size CalcualteSizeMultiply(double coeff, Size size)
        {
            return new Size((int) (size.Width * coeff), (int) (size.Height * coeff));
        }
        
        public Result<IEnumerable<(string, Size)>> GetSizesOf(Dictionary<string, int> dictionary)
        {
            return Result.Of(() =>
                {
                    var res = FormSizes(dictionary);
                    var ourSquare = GetSquare(res.Select(x => x.Item2));
                    var coeff = GetCoefficientOfLayoutSquareTo(ourSquare);
                    var result = new List<(string, Size)>();
                    foreach (var item in res)
                    {
                        var newRect = CalcualteSizeMultiply(coeff, item.Item2);
                        result.Add((item.Item1, newRect));
                    }
                    return (IEnumerable<(string, Size)>) result;
                }, "Something wrong with your size layout " +
                   "and words count. Maybe there are too " +
                   "many words for this size"
            );
        }
    }
}