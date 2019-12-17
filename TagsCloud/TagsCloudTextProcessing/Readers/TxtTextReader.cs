using System.IO;

namespace TagsCloudTextProcessing.Readers
{
    public class TxtTextReader : ITextReader
    {
        private readonly string path;

        public TxtTextReader(string path)
        {
            this.path = path;
        }

        public string ReadText()
        {
         //   if(!File.Exists())
            return File.ReadAllText(path);
        }
    }
}