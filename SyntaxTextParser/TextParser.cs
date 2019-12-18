using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultPattern;
using SyntaxTextParser.Architecture;

namespace SyntaxTextParser
{
    public sealed class TextParser
    {
        private readonly ElementParserWithRules elementParserWithRules;
        private readonly IFileReader[] fileReaders;

        public TextParser(ElementParserWithRules elementParserWithRules, IFileReader[] fileReaders)
        {
            this.elementParserWithRules = elementParserWithRules;
            this.fileReaders = fileReaders;
        }

        public Result<List<TextElement>> ParseElementsFromFile(string path, string fileName, string type)
        {
            var fullPath = Path.Combine(path, fileName + '.' + type);
            return ParseElementsFromFile(fullPath);
        }

        public Result<List<TextElement>> ParseElementsFromFile(string fullPath)
        {
            if (!File.Exists(fullPath))
                return Result.Fail<List<TextElement>>($"File {fullPath} isn't valid");

            var type = Path.GetExtension(fullPath);
            if (string.IsNullOrEmpty(type))
                return Result.Fail<List<TextElement>>($"Incorrect extension in file path {fullPath}");
            type = type.Substring(1);

            var reader = fileReaders.FirstOrDefault(x => x.CanReadThatType(type));
            if (reader == null)
                return Result.Fail<List<TextElement>>($"Parser can't read [{type}] file type");

            var readText = reader.ReadTextFromFile(fullPath);
            return readText.IsSuccess 
                ? elementParserWithRules.ParseElementsFromText(readText.GetValueOrThrow()) 
                : Result.Fail<List<TextElement>>(readText.Error);
        }
    }
}