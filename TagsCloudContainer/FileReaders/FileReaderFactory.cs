using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer.FileReaders
{
    public class FileReaderFactory : IFileReaderFactory
    {
        private readonly IEnumerable<IFileReader> fileReaders;

        public FileReaderFactory(IEnumerable<IFileReader> fileReaders)
        {
            this.fileReaders = fileReaders;
        }
        
        public Result<IFileReader> GetProperFileReader(string path)
        {
            var inputFileFormat = Path.GetExtension(path);

            foreach (var reader in fileReaders)
            {
                if (reader.SupportedFormats.Contains(inputFileFormat))
                {
                    return Result.Ok(reader);
                }
            }

            return Result.Fail<IFileReader>($"{inputFileFormat} format is not supported");
        }
    }
}