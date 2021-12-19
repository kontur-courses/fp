using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization.Infrastructure
{
    public class FileReader : IFileReader
    {
        private readonly IFileReader[] fileReaders;

        public FileReader(IEnumerable<IFileReader> fileReaders)
        {
            this.fileReaders = fileReaders.ToArray();
        }

        public bool CanReadFile(FileInfo file)
        {
            return fileReaders.Any(reader => reader.CanReadFile(file));
        }

        public Result<string> ReadFile(FileInfo file)
        {
            return CanReadFile(file) ? fileReaders.First(reader => reader.CanReadFile(file)).ReadFile(file) :
                    Result.Fail<string>("Unknown source file format.");
        }
    }
}