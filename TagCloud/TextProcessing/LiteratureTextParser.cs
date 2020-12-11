using System;
using System.IO;
using System.Linq;
using ResultOf;
using WeCantSpell.Hunspell;

namespace TagCloud.TextProcessing
{
    public class LiteratureTextParser : IWordParser
    {
        private readonly IPathCreater creater;
        private readonly ITextReader reader;
        private static readonly char[] separators = {' ', '.', ',', ':', '!', '?'};
        private const int minWordLength = 3;
        private static string[] unneccesaryWords = 
            {"мочь", "этот", "когда", "чтобы", "даже", "между", "если", "несколько", "который", "какой", "только",
                "очень", "более", "ничто", "кто", "он", "такой", "однако", "либо", "оный", "такой", "него"};
        
            
        public LiteratureTextParser(IPathCreater creater, ITextReader reader)
        {
            this.creater = creater;
            this.reader = reader;
        }
        
        public Result<string[]> GetWords(string inputFileName)
        {
            var path = creater.GetCurrentPath();
            var dictionaryResult = GetDictionary(path);
            if (!dictionaryResult.IsSuccess)
            {
                return Result.Fail<string[]>(dictionaryResult.Error);
            }
            var readStringsResult = reader.ReadStrings(path + inputFileName);
            if (!readStringsResult.IsSuccess)
            {
                return Result.Fail<string[]>(readStringsResult.Error);
            }
            
            return readStringsResult.Value
                .SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .Select(word => word.ToLower())
                .Select(word => GetRootForWord(word, dictionaryResult.Value))
                .Where(word => !(word is null))
                .Where(word => word.Length > minWordLength)
                .Where(word => !unneccesaryWords.Contains(word))
                .ToArray();
        }

        private static string GetRootForWord(string word, WordList dictionary)
        {
            return dictionary.ContainsEntriesForRootWord(word) ? word : dictionary.CheckDetails(word).Root;
        }

        private static Result<WordList> GetDictionary(string path)
        {
            try
            {
                return WordList.CreateFromFiles(path + "ru_RU.dic", path + "ru_RU.aff");
            }
            catch (FileNotFoundException e)
            {
                return Result.Fail<WordList>("Not found dictionaries (ru_RU.dic/ru_RU.aff) by path " + path);
            }
        }
    }
}