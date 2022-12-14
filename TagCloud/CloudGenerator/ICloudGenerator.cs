using System.Drawing;
using TagCloud.Infrastructure;

namespace TagCloud.CloudGenerator;

public interface ICloudGenerator
{
    public Result<Image> GenerateCloud(string filepath);
}