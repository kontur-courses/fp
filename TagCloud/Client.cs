using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class Client : IClient
    {
        private readonly IConsoleAction[] actions;
        private readonly HashSet<string> availableFontNames;
        private readonly ICloudVisualization visualization;
        private readonly Dictionary<string, IConsoleAction> actionsDictionary;
        private readonly HashSet<string> availablePaletteNames;
        private ClientConfig config;

        public Client(IConsoleAction[] actions, IPaletteNamesFactory paletteNamesFactory,
            ICloudVisualization visualization)
        {
            availableFontNames = new HashSet<string>
            {
                "Arial",
                "Comic Sans MS"
            };
            this.actions = actions;
            this.visualization = visualization;
            availablePaletteNames = paletteNamesFactory.GetPaletteNames(visualization);
            actionsDictionary = actions.ToDictionary(a => a.CommandName, a => a);
        }

        public void Start()
        {
            config = new ClientConfig(availableFontNames, availablePaletteNames, visualization);
            var userSettings = UserSettings.GetDefaultUserSettings();
            actionsDictionary["-help"].Perform(config, userSettings);
            while (!config.ToExit)
            {
                Console.WriteLine("Введите команду");
                Console.Write(">>>");
                var performResult = Result.Of(() => Console.ReadLine().ToLower())
                    .Then(command => GetCommandResult(command, actionsDictionary))
                    .Then(commandResult => commandResult.Perform(config, userSettings));
                if (!performResult.IsSuccess)
                    Console.WriteLine(performResult.Error);
            }
        }

        private Result<IConsoleAction> GetCommandResult(string commandName,
            Dictionary<string, IConsoleAction> availableCommands)
        {
            return availableCommands.TryGetValue(commandName, out var action)
                ? Result.Ok(action)
                : Result.Fail<IConsoleAction>("Unknown command");
        }
    }
}