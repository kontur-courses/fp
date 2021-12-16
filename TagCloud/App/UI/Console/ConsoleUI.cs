using TagCloud.App.UI.Console.Common;
using TagCloud.Infrastructure.FileReader;
using TagCloud.Infrastructure.Filter;
using TagCloud.Infrastructure.Lemmatizer;
using TagCloud.Infrastructure.Monad;
using TagCloud.Infrastructure.Painter;
using TagCloud.Infrastructure.Saver;
using TagCloud.Infrastructure.Weigher;

namespace TagCloud.App.UI.Console;

public class ConsoleUI : IUserInterface
{
    private readonly IFileReaderFactory fileReaderFactory;
    private readonly IFilter filter;
    private readonly ILemmatizer lemmatizer;
    private readonly IPainter painter;
    private readonly IImageSaver saver;
    private readonly IWordWeigher weigher;

    public ConsoleUI(IFileReaderFactory fileReaderFactory, IPainter painter, IWordWeigher weigher, IImageSaver saver, ILemmatizer lemmatizer, IFilter filter)
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
            .Then(x=> x.GetLines(settings.InputPath))
            .Then(lemmatizer.GetLemmas)
            .Then(filter.FilterWords)
            .Then(weigher.GetWeightedWords)
            .Then(painter.CreateImage)
            .Then(bitmap => saver.Save(bitmap, settings.OutputPath, settings.OutputFormat));

        if (!res.IsSuccess)
            System.Console.Write(res.Error);
    }
}