using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudResult.Settings;

namespace TagsCloudResult.SourceTextReaders
{
    public class TxtSourceTextReader : ISourceTextReader
    {
        private readonly ISourceFileSettings settings;

        public TxtSourceTextReader(ISourceFileSettings settings)
        {
            this.settings = settings;
        }

        public Result<IEnumerable<string>> ReadText()
        {
            return Result.Of(() => File.ReadAllLines(settings.FilePath).AsEnumerable())
                .RefineError("Не удалось считать исходный файл");
        }
    }
}
