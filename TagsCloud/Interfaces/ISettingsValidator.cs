using System.Drawing;

namespace TagsCloud.Interfaces
{
    public interface ISettingsValidator
    {
        public ResultHandler<string> VerifyFont(string settings);

        public ResultHandler<Size> VerifyPictureSize(Size size);
    }
}