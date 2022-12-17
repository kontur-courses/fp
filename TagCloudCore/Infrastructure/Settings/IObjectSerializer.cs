using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Infrastructure.Settings;

public interface IObjectSerializer
{
    Result<T> Deserialize<T>(byte[] bytes);
    Result<byte[]> Serialize<T>(T obj);
}