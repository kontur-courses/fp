using Autofac;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.TextReaders;

namespace TagsCloudVisualization.WFApp.Factories;

public class TextReaderFactory : ITextReaderFactory
{
    private readonly IComponentContext context;
    private readonly SourceSettings settings;

    public TextReaderFactory(IComponentContext context, SourceSettings settings)
    {
        this.context = context;
        this.settings = settings;
    }
    
    public Result<ITextReader> GetTextReader()
    {
        return settings.Validate()
            .Then(ResolveTextReader);
    }

    private Result<ITextReader> ResolveTextReader(SourceSettings settings)
    {
        return Result.Of(() => context.ResolveNamed<ITextReader>(Path.GetExtension(settings.Path)),
            Resources.TextReaderFactory_ResolveTextReader_Fail);
    }
}
