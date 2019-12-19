using ResultOf;
using TagCloud.Models;

namespace TagCloud
{
    public interface IConsoleAction
    {
        string CommandName { get; }
        string Description { get; }
        Result<None> Perform(ClientConfig config, UserSettings settings);
    }
}