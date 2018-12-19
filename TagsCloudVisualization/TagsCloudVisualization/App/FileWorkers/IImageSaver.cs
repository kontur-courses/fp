using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public interface IImageSaver
    {
        Result<None> WriteToFile(string fileName, Image bitmap);
    }
}
