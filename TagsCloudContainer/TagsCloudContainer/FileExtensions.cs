using CSharpFunctionalExtensions;

namespace TagsCloudContainer;

public static class FileExtensions
{
    public static Result TrySaveRandomFile(this MemoryStream cache, string directory, string extensions)
    {
        return Result.Success($"{Path.GetRandomFileName()}.{extensions}")
            .Bind(file => Path.Combine(directory, file))
            .BindIf(predicate: file => !Directory.Exists(Path.GetDirectoryName(file)), func:
                file => Result.Try(() => Directory.CreateDirectory(Path.GetDirectoryName(file)!))
                    .Bind(_ => file))
            .OnSuccessTry(file => File.WriteAllBytes(file, cache.ToArray()));
    }
}