using System.Drawing;

namespace TagsCloudForm.Common
{
    public interface IPalette
    {
        Color PrimaryColor { get; set; }

        Color SecondaryColor { get; set; }

        Color BackgroundColor { get; set; }
    }
}
