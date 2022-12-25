namespace TagsCloudVisualization.ToTagConverter;

public interface IToTagConverter
{
    Result<IEnumerable<Tag>> Convert(IEnumerable<string> words);
}