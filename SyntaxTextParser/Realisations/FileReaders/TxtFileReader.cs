using System.IO;
using Results;
using SyntaxTextParser.Architecture;

namespace SyntaxTextParser
{
    public class TxtFileReader : IFileReader
    {
        public Result<string> ReadTextFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public bool CanReadThatType(string type)
        {
            return type == "txt";
        }
    }
}