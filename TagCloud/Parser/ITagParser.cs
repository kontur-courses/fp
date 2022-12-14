using TagCloud.Infrastructure;

namespace TagCloud.Parser;

public interface ITagParser
{
    public Result<TagMap> Parse(string filepath);
}