using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.DocIO;

namespace TagsCloudVisualization.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public string FilePath { get; }

        public TxtFileReader(string filePath)
        {
            FilePath = filePath;
        }

        public bool TryReadAllText(out string text)
        {
            text = string.Empty;
            try
            {
                text = File.ReadAllText(FilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
