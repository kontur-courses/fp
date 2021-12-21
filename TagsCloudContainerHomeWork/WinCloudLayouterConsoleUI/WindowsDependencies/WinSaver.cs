using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using TagsCloudContainerCore.InterfacesCore;

namespace WinCloudLayouterConsoleUI.WindowsDependencies;

[SuppressMessage("Interoperability", "CA1416", MessageId = "Проверка совместимости платформы")]
public class WinSaver : IBitmapHandler
{
    public static readonly Dictionary<string, ImageFormat> SupportedFormats = new()
    {
        { "png", ImageFormat.Png },
        { "jpg", ImageFormat.Jpeg },
        { "jpeg", ImageFormat.Jpeg },
        { "bmp", ImageFormat.Bmp }
    };

    private static readonly Regex NameRegex = new(@"(?<=[\\\/])[^\\\/]+?\.(.+)$", RegexOptions.Compiled);

    public void Handle(Bitmap picture, string outPath, string format)
    {
        if (!NameRegex.IsMatch(outPath))
        {
            var name = DateTime.Now.ToString("dd-MMMM-yyyy-hh-mm") + ".png";

            picture.Save((outPath + "\\" + name).Replace("\"", ""), ImageFormat.Png);

            return;
        }

        if (!SupportedFormats.ContainsKey(format))
        {
            throw new FormatException("Неподдерживаемый формат изображения");
        }

        picture.Save(outPath, SupportedFormats[format]);
    }
}