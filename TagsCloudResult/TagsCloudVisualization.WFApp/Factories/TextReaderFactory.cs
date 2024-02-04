using Autofac;
using TagsCloudVisualization.Common;
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
        return ValidatePathNotNullOrEmpty(settings.Path)
            .Then(ValidatePathExists)
            .Then(_ => ResolveTextReader());
    }
    
    private static Result<string> ValidatePathNotNullOrEmpty(string path)
    {
        return Result.Validate(path, path => !string.IsNullOrEmpty(path),
            "В качестве пути к файлу источника была предоставлена пустая строка. Пожалуйста, укажите верный путь к файлу.");
    }

    private static Result<string> ValidatePathExists(string path)
    {
        return Result.Validate(path, File.Exists,
            $@"Файла источника по указанному пути не существует: {Path.GetFullPath(path)}");
    }

    private Result<TextReader> ResolveTextReader()
    {
        return Result.Of(
            () => context.ResolveNamed<TextReader>(Path.GetExtension(settings.Path)),
            "Указанный тип файлов не поддерживается в качестве источника.");
    }
}