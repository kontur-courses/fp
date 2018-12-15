using System.Collections.Generic;
using Result;
using TagCloudCreation;

namespace TagCloudApp
{
    internal class SelectedBoringWordsRemover : IWordPreparer
    {
        private static ITextReader textReader;
        private HashSet<string> boringWords;

        public SelectedBoringWordsRemover(ITextReader reader)
        {
            textReader = reader;
        }

        public Result<Maybe<string>> PrepareWord(string word, TagCloudCreationOptions options)
        {
            return InitializeBoringWords(options)
                .Then(r => r.Contains(word) ? Maybe<string>.None : word);
        }

        public Result<HashSet<string>> InitializeBoringWords(TagCloudCreationOptions options)
        {
            if (options.PathToBoringWords.HasNoValue)
                return Result.Result.Fail<HashSet<string>>("No path was given");
            return boringWords == null ? ReadWords(options.PathToBoringWords.Value) : Result.Result.Ok(boringWords);
        }

        private Result<HashSet<string>> ReadWords(string path)
        {
            return textReader.ReadWords(path)
                             .Then(r => boringWords = new HashSet<string>(r));
        }
    }
}
