using Autofac;
using TagsCloudVisualization.Common.ResultOf;
using TagsCloudVisualization.TextReaders;
using TextReader = TagsCloudVisualization.TextReaders.TextReader;

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
    
    public Result<TextReader> GetTextReader()
    {
        return settings.Validate()
            .Then(ResolveTextReader);
    }

    private Result<TextReader> ResolveTextReader(SourceSettings settings)
    {
        return Result.Of(() => context.ResolveNamed<TextReader>(Path.GetExtension(settings.Path)),
            "Указанный тип файлов не поддерживается в качестве источника.");
    }
}
