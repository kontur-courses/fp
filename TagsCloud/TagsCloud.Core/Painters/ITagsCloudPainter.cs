using System.Drawing;
using TagsCloud.Core.TagContainersProviders;

namespace TagsCloud.Core.Painters;

public interface ITagsCloudPainter
{
	public Result<Bitmap> Draw(List<TagContainer> tagContainers);
}