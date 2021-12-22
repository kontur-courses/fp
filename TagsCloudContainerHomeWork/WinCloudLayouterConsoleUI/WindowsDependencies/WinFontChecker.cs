using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using TagsCloudContainerCore.InterfacesCore;

namespace WinCloudLayouterConsoleUI.WindowsDependencies;

public class WinFontChecker : IFontChecker
{
    [SuppressMessage("Performance", "CA1822", MessageId = "Пометьте члены как статические")]
    public bool IsFontInstalled(string fontName)
    {
        using var checkFont = new Font(fontName, 10);
        return checkFont.Name.ToLowerInvariant().Equals(fontName.ToLowerInvariant());
    }
}