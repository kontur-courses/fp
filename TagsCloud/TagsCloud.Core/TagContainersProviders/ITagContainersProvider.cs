namespace TagsCloud.Core.TagContainersProviders;

public interface ITagContainersProvider
{
	public List<Result<TagContainer>> GetContainers(int? count = null);
}