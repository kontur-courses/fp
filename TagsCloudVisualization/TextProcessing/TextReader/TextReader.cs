using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudVisualization.TextProcessing.Readers;

namespace TagsCloudVisualization.TextProcessing.TextReader
{
    public class TextReader : ITextReader
    {
        private readonly List<IReader> readers;
        
        public TextReader(IEnumerable<IReader> readers)
        {
            this.readers = readers.ToList();
        }

        public Result<string> ReadAllText(string path)
        {
            return !File.Exists(path) 
                ? Result.Fail<string>($"File {path} does not exist") 
                : GetTextFromReader(path);
        }

        private Result<string> GetTextFromReader(string path)
        {
            var reader = readers.FirstOrDefault(rdr => rdr.CanReadFile(path));
            return reader == null 
                ? Result.Fail<string>($"Extension {Path.GetExtension(path)} doesn't support") 
                : reader.ReadText(path);
        }
    }
}