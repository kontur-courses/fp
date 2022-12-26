using System;
using System.Collections.Generic;
using TagsCloudVisualization.TextReaders;
using TagsCloudVisualization.WordProcessors;

namespace TagsCloudVisualization.Storages
{
    public class FilteredWordStorage : IWordStorage
    {
        public IEnumerable<string> Words { get; set; }

        public FilteredWordStorage(ITextReader reader, IWordProcessor filter)
        {
            var readingResult = reader.Read();
            if (!readingResult.IsSuccess)
                return;
            Words = filter.Process(readingResult.Value).Value;
        }
    }
}
