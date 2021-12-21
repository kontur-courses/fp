using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainerCore.InterfacesCore;

// ReSharper disable once UnusedType.Global
public interface IPainter
{
    // ReSharper disable once UnusedMember.Global
    public Bitmap Paint(IEnumerable<TagToRender> tags);
}