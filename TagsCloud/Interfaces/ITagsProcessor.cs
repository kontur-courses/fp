using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
	public interface ITagsProcessor
	{
		Result<IEnumerable<Tag>> GetTags();
	}
}