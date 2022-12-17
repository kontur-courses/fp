using System.Xml.Serialization;
using TagCloudCore.Infrastructure.Results;

namespace TagCloudCore.Infrastructure.Settings;

public class XmlObjectSerializer : IObjectSerializer
{
    public Result<T> Deserialize<T>(byte[] bytes) =>
        Result.Of<T>(() =>
            {
                using var ms = new MemoryStream(bytes);
                return (T) new XmlSerializer(typeof(T)).Deserialize(ms)!;
            }
        );

    public Result<byte[]> Serialize<T>(T obj)
    {
        return Result.Of(() =>
            {
                using var ms = new MemoryStream();
                new XmlSerializer(typeof(T)).Serialize(ms, obj);
                return ms.ToArray();
            }
        );
    }
}