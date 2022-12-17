using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Infrastructure.Settings;

public class FileBlobStorage : IBlobStorage
{
    public Result<byte[]> Get(string name) =>
        File.Exists(name)
            ? Result.Of(() => File.ReadAllBytes(name), $"Unable to read file: {name}.")
            : Result.Fail<byte[]>($"File not found: {name}.");

    public Result<None> Set(string name, byte[] content) =>
        Result.OfAction(() => File.WriteAllBytes(name, content), $"Unable to write file: {name}.");
}