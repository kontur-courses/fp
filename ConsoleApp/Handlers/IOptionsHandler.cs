using ConsoleApp.Options;
using TagsCloudContainer;

namespace ConsoleApp.Handlers;

public interface IOptionsHandler
{
    public bool CanParse(IOptions options);
    
    public Result<string> WithParsed(IOptions options);
}