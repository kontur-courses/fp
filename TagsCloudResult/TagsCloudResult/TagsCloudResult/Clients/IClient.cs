using ResultOf;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IClient
    {
        Result<Color> GetTextColor();
        Result<Color> GetBackgoundColor();
        Result<FontFamily> GetFontFamily();
        Result<Size> GetImageSize();
        void ShowPathToNewFile(string path);
        Result<string> GetNameForImage();
        void ShowMessage(string message);
        bool IsFinish();
    }
}
