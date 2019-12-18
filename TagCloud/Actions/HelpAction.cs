using System;
using System.Linq;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class HelpAction : IAction
    {
        private readonly IAction[] actions;
        public string CommandName { get; } = "-help";

        public string Description { get; } = "показать все команды";

        public HelpAction(IAction[] actions)
        {
            this.actions = actions;
        }

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            foreach (var action in actions)
                Console.WriteLine($"{action.CommandName}     \"{action.Description}\"");
            return Result.Ok();
        }
    }
}
