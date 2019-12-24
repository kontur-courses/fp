using System;
using System.IO;

namespace TagsCloudContainer.Models
{
    public class InputInfo
    {
        public readonly string TextFileName;
        public readonly int MaxWordsCnt;
        public readonly string ImageFormat;
        public readonly string ImageFileName;

        public InputInfo(string textFileName="", string imageFileName = "",
            int maxWordsCnt = int.MaxValue, string imageFormat = "png")
        {
            TextFileName = textFileName;
            MaxWordsCnt = maxWordsCnt;
            ImageFormat = imageFormat;
            if (imageFileName == null)
                ImageFileName = Path.Combine(new string[]
                    {AppDomain.CurrentDomain.BaseDirectory, "tagCloud"});
            else
                ImageFileName = imageFileName;
        }
    }
}