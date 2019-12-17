using FailuresProcessing;
using System.Drawing;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.Settings
{
    public class PainterSettings : IPainterSettings
    {
        public PainterSettings() => Reset();

        public string[] ColorsNames { get; set; }
        public string BackgroundColorName { get; set; }

        public virtual Result<None> Reset()
        {
            ColorsNames = new[] { Color.Red.Name, Color.Yellow.Name, Color.Cyan.Name };
            BackgroundColorName = Color.Black.Name;
            return Result.Ok();
        }
    }
}