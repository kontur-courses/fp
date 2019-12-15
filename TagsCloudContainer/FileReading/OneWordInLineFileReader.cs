using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer.FileReading
{
    public class OneWordInLineFileReader : IFileReader
    {
        public IEnumerable<string> ReadWords(string textFileName)
        {
            if (!File.Exists(textFileName))
                throw new FileNotFoundException($"File {textFileName} was not found");
            return File.ReadLines(textFileName);
        }
    }
}