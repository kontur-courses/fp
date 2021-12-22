using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using TagsCloudContainerCore.InterfacesCore;
using TagsCloudContainerCore.Result;

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

    public Result<None> Handle(Bitmap picture, string outPath, string format)
    {
        var name = DateTime.Now.ToString("dd-MMMM-yyyy-hh-mm") + ".png";
        outPath = NameRegex.IsMatch(outPath) ? outPath : (outPath + "\\" + name).Replace("\"", "");

        if (!SupportedFormats.ContainsKey(format))
        {
            return ResultExtension.Fail<None>("Неподдерживаемый формат изображения");
        }

        try
        {
            picture.Save(outPath, SupportedFormats[format]);
            return ResultExtension.Ok();
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<None>($"{e.GetType().Name} {e.Message}");
        }
    }
}