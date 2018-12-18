using TagsCloudContainer.ResultOf;

namespace TagsCloudContainer.ImageWriter
{
    public interface IImageWriter
    {
        Result<None> Write(byte[] image, string pathToSave);
    }
}