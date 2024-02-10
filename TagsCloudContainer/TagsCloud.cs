namespace TagsCloudContainer;

public class TagsCloud: ITagCloud
{
    public TagsCloud(List<Tag> tags)
    {
        Tags = tags;
    }

    public List<Tag> Tags { get; set; }
}