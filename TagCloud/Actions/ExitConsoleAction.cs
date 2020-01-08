using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class ExitConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-exit";

        public string Description { get; } = "exit the program";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            config.ToExit = true;
            return Result.Ok();
        }
    }
}