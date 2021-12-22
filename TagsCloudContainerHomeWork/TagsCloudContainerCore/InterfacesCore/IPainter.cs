using System.Collections.Generic;
using System.Drawing;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.InterfacesCore;

// ReSharper disable once UnusedType.Global
public interface IPainter
{
    // ReSharper disable once UnusedMember.Global
    public Result<Bitmap> Paint(IEnumerable<TagToRender> tags);
}