using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization.Infrastructure.Text
{
    public class ReaderContainer : ITextReader
    {
        private readonly ITextReader[] fileReaders;

        public ReaderContainer(IEnumerable<ITextReader> fileReaders)
        {
            this.fileReaders = fileReaders.ToArray();
        }

        public bool CanReadFile(FileInfo file)
        {
            return fileReaders.Any(reader => reader.CanReadFile(file));
        }

        public Result<string> ReadFile(FileInfo file)
        {
            return CanReadFile(file) 
                ? fileReaders.First(reader => reader.CanReadFile(file)).ReadFile(file) 
                : Result.Fail<string>("Unknown source file format.");
        }
    }
}