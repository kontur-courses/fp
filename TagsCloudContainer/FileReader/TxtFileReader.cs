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

        public Result<IEnumerable<string>> Read()
        {
            return Result.Of(
                () => File.ReadAllLines(provider.WordsFilePath)
                .Where(str => str != ""))
                .RefineError("Can't read file " + provider.WordsFilePath);
            //return new Result<IEnumerable<string>>(result.Value, result.Error);
        }
    }
}