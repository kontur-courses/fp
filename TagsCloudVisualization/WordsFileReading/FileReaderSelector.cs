using System;
using System.Collections.Generic;
using ResultOf;

namespace TagsCloudVisualization.WordsFileReading
{
    public class FileReaderSelector
    {
        private readonly IDictionary<string, IFileReader> fileReaderByExtension;

        public FileReaderSelector(IEnumerable<IFileReader> fileReaders)
        {
            fileReaderByExtension = new Dictionary<string, IFileReader>();

            foreach (var reader in fileReaders)
                foreach (var extension in reader.SupportedTypes())
                    fileReaderByExtension[extension] = reader;
        }

        public Result<IFileReader> SelectFileReader(string fileName)
        {
            var extension = fileName.ExtractFileExtension();
            if (extension != null && fileReaderByExtension.TryGetValue(extension, out var fileReader))
                return Result.Ok(fileReader);

            return Result.Fail<IFileReader>("Input file format is not supported");
        }
    }
}
