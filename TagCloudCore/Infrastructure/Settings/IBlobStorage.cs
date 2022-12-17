using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Infrastructure.Settings;

public interface IBlobStorage
{
    Result<byte[]> Get(string name);
    Result<None> Set(string name, byte[] content);
}