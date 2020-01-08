using System;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class HelpConsoleAction : IConsoleAction
    {
        private readonly IConsoleAction[] actions;

        public HelpConsoleAction(IConsoleAction[] actions)
        {
            this.actions = actions;
        }

        public string CommandName { get; } = "-help";

        public string Description { get; } = "показать все команды";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return Result.OfAction(() =>
            {
                Console.WriteLine("Список доступных комманд :");
                foreach (var action in actions)
                    Console.WriteLine($"{action.CommandName}     \"{action.Description}\"");
                Console.WriteLine($"{CommandName}      \"{Description}\"");
            });
        }
    }
}