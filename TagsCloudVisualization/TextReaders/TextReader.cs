using System;
using System.IO;

namespace TagsCloudVisualization.TextReaders
{
    internal class TextReader : ITextReader
    {
        public string Path { get; }

        public TextReader(string path)
        {
            Path = path;
        }

        public Result<string[]> Read()
        {
            if (!File.Exists(Path)) 
                return Result.Fail<string[]>("File at the specified path does not exist");
            
            var textResult = Result.Of(() => File.ReadAllText(Path));

            if (!textResult.IsSuccess)
                return Result.Fail<string[]>("Failed to read file");

            var text = textResult.Value;
            var separators = new[]
            {
                ' ', ',', '.', ';', ':', '<', '>', '[', ']',
                '{', '}', '"', '(', ')','\'', '~',
                '?', '!','$', '|', '_', '&', '#', '@', '^', '%',
                '+', '=', '-', '/', '*', '\\', '|',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
                '\n', '\r', '\t'
            };

            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
