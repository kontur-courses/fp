using System.IO;
using ResultPattern;
using SyntaxTextParser.Architecture;

namespace SyntaxTextParser
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadTextFromFile(string filePath)
        {
            return File.ReadAllText(filePath).AsResult();
        }

        public bool CanReadThatType(string type)
        {
            return type == "txt";
        }
    }
}