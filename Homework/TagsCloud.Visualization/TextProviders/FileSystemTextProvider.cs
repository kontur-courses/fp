using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloud.Visualization.Extensions;
using TagsCloud.Visualization.TextProviders.FileReaders;
using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TextProviders
{
    public class FileSystemTextProvider : ITextProvider
    {
        private readonly string fileName;
        private readonly IEnumerable<IFileReader> fileReaders;

        public FileSystemTextProvider(string fileName, IEnumerable<IFileReader> fileReaders)
        {
            this.fileName = fileName;
            this.fileReaders = fileReaders;
        }

        public Result<string> Read()
        {
            return fileName.AsResult()
                .Validate(f => f != null, nameof(fileName))
                .Validate(File.Exists, $"File {fileName} doesn't exists")
                .Then(f => Path.GetExtension(f)?.Replace(".", ""))
                .Validate(ext => ext != null, ext => $"Unknown extension of file: {ext}")
                .Then(ext => fileReaders.FirstOrDefault(x => x.CanRead(ext)))
                .Validate(x => x != null, "Unsupported file extension")
                .Then(x => x.Read(fileName));
        }
    }
}