using CSharpFunctionalExtensions;

namespace TagsCloudContainer;

public static class FileExtensions
{
    public static Result TrySaveRandomFile(this MemoryStream cache, string directory, string extensions)
    {
        return Result.Success($"{Path.GetRandomFileName()}.{extensions}")
            .Bind(file => Path.Combine(directory, file))
            .BindIf(file => !Directory.Exists(Path.GetDirectoryName(file)), file => Result
                .Try(() => Directory.CreateDirectory(Path.GetDirectoryName(file)!))
                .Bind(_ => file))
            .OnSuccessTry(file => File.WriteAllBytes(file, cache.ToArray()));
    }

    public static Result CheckWritingAccessToDirectory(string path)
    {
        var f = Path.Combine(path, Path.GetRandomFileName());
        return Directory.Exists(path)
            ? Result.Try(() =>
            {
                using var _ = File.Create(f, 1, FileOptions.DeleteOnClose);
            })
            : Result.Success();
    }
}