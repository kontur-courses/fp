using TagCloud.Infrastructure.FileReader;
using TagCloud.Infrastructure.Filter;
using TagCloud.Infrastructure.Lemmatizer;
using TagCloud.Infrastructure.Monad;
using TagCloud.Infrastructure.Painter;
using TagCloud.Infrastructure.Pipeline.Common;
using TagCloud.Infrastructure.Saver;
using TagCloud.Infrastructure.Weigher;

namespace TagCloud.Infrastructure.Pipeline;

public class ImageProcessor : IImagePipeline
{
    private readonly IFileReaderFactory fileReaderFactory;
    private readonly IFilter filter;
    private readonly ILemmatizer lemmatizer;
    private readonly IPainter painter;
    private readonly IImageSaver saver;
    private readonly IWordWeigher weigher;

    public ImageProcessor(IFileReaderFactory fileReaderFactory, IPainter painter, IWordWeigher weigher, IImageSaver saver, ILemmatizer lemmatizer, IFilter filter)
    {
        this.fileReaderFactory = fileReaderFactory;
        this.painter = painter;
        this.weigher = weigher;
        this.saver = saver;
        this.lemmatizer = lemmatizer;
        this.filter = filter;
    }

    public void Run(IAppSettings settings)
    {
        var res = fileReaderFactory
            .Create(settings.InputPath)
            .Then(fileReader=> fileReader.GetLines(settings.InputPath))
            .Then(lemmatizer.GetLemmas)
            .Then(filter.FilterWords)
            .Then(weigher.GetWeightedWords)
            .Then(painter.CreateImage)
            .Then(bitmap => saver.Save(bitmap, settings.OutputPath, settings.OutputFormat));

        if (!res.IsSuccess)
            Console.Write(res.Error);
    }
}