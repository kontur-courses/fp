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
        Result<FileSaveResult> WriteToFile(string fileName, Image bitmap);
    }
}
