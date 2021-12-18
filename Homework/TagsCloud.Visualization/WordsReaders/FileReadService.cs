using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloud.Visualization.Utils;
using TagsCloud.Visualization.WordsReaders.FileReaders;

namespace TagsCloud.Visualization.WordsReaders
{
    public class FileReadService : IWordsReadService
    {
        private readonly string fileName;
        private readonly IEnumerable<IFileReader> fileReaders;

        public FileReadService(string fileName, IEnumerable<IFileReader> fileReaders)
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