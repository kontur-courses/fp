using System;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer.TextPreparation
{
    public class TxtFileReader : IFileReader
    {
        private readonly IWordsReader wordsReader;

        public TxtFileReader(IWordsReader wordsReader)
        {
            this.wordsReader = wordsReader;
        }

        public Result<List<string>> GetAllWords(string filePath)
        {
            if (filePath == null)
            {
                return Result.Fail<List<string>>("Path can't be null");
            }

            using var reader = new StreamReader(filePath);
            return wordsReader.ReadAllWords(reader.ReadToEnd());
        }
    }
}