using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloud.ImageProcessing.Config
{
    public interface IImageConfig
    {
        Size ImageSize { get; set; }
        string Path { get; set; }
    }
}
