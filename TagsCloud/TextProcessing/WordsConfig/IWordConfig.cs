using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.TextProcessing.WordsConfig
{
    public interface IWordConfig
    {
        Font Font { get; set; }
        Color Color { get; set; }
        string Path { get; set; }
        string[] FilersNames { get; set; }
        string[] ConvertersNames { get; set; }
        string LayouterName { get; set; }
        string TagGeneratorName { get; set; }
    }
}
