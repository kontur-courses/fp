using System.IO;

namespace TagCloud.Words
{
    public class FileReader : IReader
    {
        public string Read(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}