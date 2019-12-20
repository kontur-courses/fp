using System.Collections.Generic;
using System.Linq;
using FunctionalTools;

namespace TagsCloudGenerator.FileReaders
{
    public class ReaderFactory : IReaderFactory
    {
        private readonly Dictionary<string, IFileReader> extensionToReader;

        public ReaderFactory(IEnumerable<IFileReader> extensionToReader)
        {
            this.extensionToReader = extensionToReader.ToDictionary(x => x.TargetExtension, x => x);
        }

        public Result<IFileReader> GetReaderFor(string extension)
        {
            return extensionToReader.TryGetValue(extension, out var reader)
                ? Result.Ok(reader)
                : Result.Fail<IFileReader>($"Unsupported extension {extension}");
        }
    }
}