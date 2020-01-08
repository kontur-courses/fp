using System;
using System.Collections.Generic;
using System.IO;
using ResultOf;
using TagCloud.Factories;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class TagCollectionFactory : ITagCollectionFactory
    {
        private readonly IFileReader fileReader;
        private readonly IWordsToTagsParser parser;
        private readonly IWordsHandler wordsHandler;

        public TagCollectionFactory(IWordsHandler wordsHandler, IWordsToTagsParser parser,
            IFileReader fileReader)
        {
            this.wordsHandler = wordsHandler;
            this.parser = parser;
            this.fileReader = fileReader;
        }

        public Result<List<Tag>> Create(ImageSettings imageSettings, string path)
        {
            return fileReader.ReadWordsFromFile(path)
                .Then(words => wordsHandler.GetWordsAndCount(words))
                .Then(wordsAndCount =>
                    wordsHandler.RemoveBoringWords(wordsAndCount, $"{GetCurrentDirectoryPath()}\\BoringWords.txt"))
                .Then(wordsAfterConversion => parser.GetTags(wordsAfterConversion, imageSettings));
        }

        private string GetCurrentDirectoryPath()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
                return directoryInfo.FullName;
            throw new ArgumentException("directory not exists");
        }
    }
}