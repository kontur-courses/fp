using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class SettingsValidator : ISettingsValidator
    {
        public ResultHandler<string> VerifyFont(string fontName)
        {
            var installedFonts = new InstalledFontCollection();
            var fontFamily = installedFonts.Families;
            var handler = new ResultHandler<string>(fontName);

            if (fontFamily.Any(x => x.Name == fontName))
            {
                return handler;
            }

            return handler.Fail("Font was not found");
        }

        public ResultHandler<Size> VerifyPictureSize(Size size)
        {
            var handler = new ResultHandler<Size>(size);

            if (size.Width > 0 && size.Height > 0)
            {
                return handler;
            }

            return handler.Fail("The size of the picture is wrong");
        }
    }
}