using System;
using System.IO;
using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TextProcessing.TextReaders
{
    public class ReadersFactory : ServiceFactory<IWordsReader>
    {
        private readonly WordConfig wordsConfig;

        public ReadersFactory(WordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<IWordsReader> Create()
        {
            var reader = services.FirstOrDefault(pair => pair.Value().CanRead(wordsConfig.Path)).Value;
            if (!File.Exists(wordsConfig.Path))
                return Result.Fail<IWordsReader>($"Path: {wordsConfig.Path} don't exists");

            return Result.Of(reader, $"This file type {wordsConfig.Path.Split('.').Last()} is not supported");
        }
    }
}
