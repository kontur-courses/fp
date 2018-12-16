using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.FileReader
{
    public class TxtFileReader : IFileReader
    {
        private readonly IFilePathProvider provider;

        public TxtFileReader(IFilePathProvider provider)
        {
            this.provider = provider;
        }

        public ReadFileResult Read()
        {
            var result = Result.Of(
                () => File.ReadAllLines(provider.WordsFilePath)
                .Where(str => str != ""))
                .RefineError("Can't read file " + provider.WordsFilePath);
            return new ReadFileResult(result.Value, result.Error);
        }
    }
}