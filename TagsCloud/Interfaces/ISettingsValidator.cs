using System.Drawing;

namespace TagsCloud.Interfaces
{
    public interface ISettingsValidator
    {
        public Result<string> VerifyFont(string settings);

        public Result<Size> VerifyPictureSize(Size size);
    }
}
