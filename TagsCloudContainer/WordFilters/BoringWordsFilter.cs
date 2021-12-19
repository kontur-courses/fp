using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.WordFilters
{
    public class BoringWordsFilter : IWordsFilter
    {
        private HashSet<string> boringWords;
        private readonly string boringWordsPath;
        private readonly IFileReaderFactory fileReaderFactory;

        public BoringWordsFilter(IAppSettings appSettings, IFileReaderFactory fileReaderFactory)
        {
            boringWords = new HashSet<string>();
            boringWordsPath = appSettings.BoringWordsPath;
            this.fileReaderFactory = fileReaderFactory;
        }

        public Result<List<string>> Filter(IEnumerable<string> words)
        {
            if (!boringWords.Any())
            {
                var boringWordsResult = ReadBoringWords();
                if (boringWordsResult.IsSuccess)
                {
                    boringWords = boringWordsResult.Value.ToHashSet();
                }
                else
                    return Result.Fail<List<string>>(boringWordsResult.Error);
            }

            return words.Where(word => !boringWords.Contains(word)).ToList().AsResult();
        }

        private Result<IEnumerable<string>> ReadBoringWords()
        {
            return fileReaderFactory.GetProperFileReader(boringWordsPath)
                .Then(reader => reader.ReadWordsFromFile(boringWordsPath));
        }
    }
}