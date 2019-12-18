using System.Collections.Generic;
using System.Linq;

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
            return Result
                .Of(() => extensionToReader[extension])
                .ReplaceError(err => $"Unsupported extension {extension}");
        }
    }
}