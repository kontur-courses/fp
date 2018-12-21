using System.Collections.Generic;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Layout;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.Drawing
{
    public interface IDrawer
    {
        Result<byte[]> Draw(HashSet<Tag> tags, ImageSettings settings);
    }
}