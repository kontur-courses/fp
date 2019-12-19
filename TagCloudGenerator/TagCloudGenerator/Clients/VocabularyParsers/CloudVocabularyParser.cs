using System.Collections.Generic;
using System.IO;
using TagCloudGenerator.ResultPattern;

namespace TagCloudGenerator.Clients.VocabularyParsers
{
    public abstract class CloudVocabularyParser : ICloudVocabularyParser
    {
        private readonly CloudVocabularyParser nextParser;

        protected CloudVocabularyParser(CloudVocabularyParser nextParser) => this.nextParser = nextParser;

        public Result<IEnumerable<string>> GetCloudVocabulary(string cloudVocabularyFilename)
        {
            if (!File.Exists(cloudVocabularyFilename))
                return Result.Fail<IEnumerable<string>>(
                    $"Specified file path '{cloudVocabularyFilename}' doesn't exist.");

            if (!VerifyFilename(cloudVocabularyFilename))
            {
                if (nextParser is null)
                    return Result.Fail<IEnumerable<string>>(
                        $@"Invalid vocabulary filename format: specified filename not supported '{
                            cloudVocabularyFilename}'.");

                nextParser.GetCloudVocabulary(cloudVocabularyFilename);
            }

            return Result.Of(() => File.OpenText(cloudVocabularyFilename))
                .Then(ParseCloudVocabulary);
        }

        protected abstract bool VerifyFilename(string filePath);
        protected abstract IEnumerable<string> ParseCloudVocabulary(StreamReader vocabularyFileStream);
    }
}