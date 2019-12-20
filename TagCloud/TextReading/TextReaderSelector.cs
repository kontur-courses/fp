using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;

namespace TagCloud.TextReading
{
    public class TextReaderSelector : ITextReaderSelector
    {
        private readonly IEnumerable<ITextReader> textReaders;

        public TextReaderSelector(IEnumerable<ITextReader> textReaders)
        {
            this.textReaders = textReaders;
        }

        public Result<ITextReader> GetTextReader(FileInfo file)
        {
            var reader = textReaders.FirstOrDefault(r => r.Extension == file.Extension);
            return reader == null 
                ? Result.Fail<ITextReader>($"{file.Extension} files are not supported") 
                : Result.Ok(reader);
        }
    }
}