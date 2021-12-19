namespace TagsCloudContainer.Appliers;

public interface IPreprocessorsApplier
{
    IEnumerable<string> ApplyPreprocessors(IEnumerable<string> words);
}
