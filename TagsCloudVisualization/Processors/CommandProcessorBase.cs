using Autofac;
using TagsCloudVisualization.Common.FileReaders;
using TagsCloudVisualization.Common.ImageWriters;
using TagsCloudVisualization.Common.TagCloudPainters;
using TagsCloudVisualization.Common.Tags;
using TagsCloudVisualization.Common.TextAnalyzers;

namespace TagsCloudVisualization.Processors
{
    public class CommandProcessorBase<T> : ICommandProcessor<T>
    {
        protected readonly IFileReader fileReader;
        protected readonly ITextAnalyzer textAnalyzer;
        protected readonly ITagBuilder tagBuilder;
        protected readonly ITagCloudPainter tagCloudPainter;
        protected readonly IImageWriter imageWriter;

        protected CommandProcessorBase(IFileReader fileReader, ITextAnalyzer textAnalyzer, ITagBuilder tagBuilder,
            ITagCloudPainter tagCloudPainter, IImageWriter imageWriter)
        {
            this.fileReader = fileReader;
            this.textAnalyzer = textAnalyzer;
            this.tagBuilder = tagBuilder;
            this.tagCloudPainter = tagCloudPainter;
            this.imageWriter = imageWriter;
        }

        public virtual int Run(T options)
        {
            return 0;
        }
    }
}