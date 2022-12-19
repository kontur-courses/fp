using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class SettingsValidator : ISettingsValidator
    {
        public Result<string> VerifyFont(string fontName)
        {
            var installedFonts = new InstalledFontCollection();
            var fontFamily = installedFonts.Families;

            if (fontFamily.Any(x => x.Name == fontName))
            {
                return Result.Ok(fontName);
            }

            return Result.Fail<string>("Font was not found");
        }

        public Result<Size> VerifyPictureSize(Size size)
        {
            if (size.Width > 0 && size.Height > 0)
            {
                return Result.Ok(size);   
            }

            return Result.Fail<Size>("The size of the picture is wrong");
        }
    }
}
