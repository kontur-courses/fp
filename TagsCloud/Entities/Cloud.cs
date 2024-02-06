using System.Drawing;

namespace TagsCloud.Entities;

public class Cloud
{
    public IEnumerable<Tag> Tags { get; private set; }
    public Size CloudSize { get; private set; }

    public Cloud(IEnumerable<Tag> tags, Size cloudSize)
    {
        Tags = tags;
        CloudSize = cloudSize;
    }
}