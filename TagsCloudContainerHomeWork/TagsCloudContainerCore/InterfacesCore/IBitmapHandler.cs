using System.Drawing;

namespace TagsCloudContainerCore.InterfacesCore;

// ReSharper disable once UnusedType.Global
public interface IBitmapHandler
{
    // ReSharper disable once UnusedMember.Global
    public void Handle(Bitmap bitmap, string outPath, string format);
}