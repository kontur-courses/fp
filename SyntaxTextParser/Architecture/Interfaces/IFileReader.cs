using Results;

namespace SyntaxTextParser.Architecture
{
    public interface IFileReader
    {
        Result<string> ReadTextFromFile(string filePath);

        bool CanReadThatType(string type);
    }
}