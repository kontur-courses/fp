using System;
using CommandLine;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudContainer
{
    public class StandartOptions
    {
        [Value(1, HelpText = "File to take words from")]
        public string File { get; set; }
        [Option('c', "count", Required = false, Default = int.MaxValue, HelpText = "Count of words in tag cloud")]
        public int MaxCnt { get; set; }

        [Option('f', "format", Required = false, Default = "png", HelpText = "Format of tag cloud file")]
        public string Format
        {
            get { return format; }
            set
            {
                if (!PictureInfo.availableFormats.ContainsKey(value))
                    throw new ArgumentException("\nNot available format.\n" +
                                                "Available formats are:\n" +
                                                string.Join("\n", PictureInfo.availableFormats.Keys.Select(x => x.ToString())));
                format = value;
            }
        }

        private string format;
    }
}