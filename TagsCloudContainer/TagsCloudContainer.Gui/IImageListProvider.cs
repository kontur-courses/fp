using CSharpFunctionalExtensions;

namespace TagsCloudContainer.Gui;

public interface IImageListProvider
{
    Result AddImageBits(byte[] imageBytes);
}