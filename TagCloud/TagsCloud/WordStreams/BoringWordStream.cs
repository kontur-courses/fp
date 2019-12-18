using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.FileReader;
using TagsCloud.Interfaces;
using TagsCloud.Splitters;

namespace TagsCloud.WordStreams
{
    public class BoringWordStream
    {
        private readonly ITextReader fileReader;
        private readonly ITextSplitter textSplitter;
        private readonly IWordHandler wordHandler;

        public BoringWordStream(IWordHandler wordHandler, SplitterByLine textSplitter, TxtReader fileReader)
        {
            this.textSplitter = textSplitter;
            this.wordHandler = wordHandler;
            this.fileReader = fileReader;
        }

        public Result<IEnumerable<string>> GetWords(string path)
        {
            return fileReader.ReadFile(path)
                .Then(textSplitter.SplitText)
                .Then(words => words.Select(word => wordHandler.ProcessWord(word)));
        }
    }
}