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
        private readonly IPaletteNamesFactory paletteNamesFactory;
        private readonly ICloudVisualization visualization;
        private readonly HashSet<string> availableFontNames;
        private HashSet<string> availablePaletteNames;
        private ClientConfig config;

        public Client(IConsoleAction[] actions, IPaletteNamesFactory paletteNamesFactory, ICloudVisualization visualization)
        {
            availableFontNames = new HashSet<string>
            {
                "Arial",
                "Comic Sans MS"
            };
            this.actions = actions;
            this.paletteNamesFactory = paletteNamesFactory;
            this.visualization = visualization;
            availablePaletteNames = new HashSet<string>();
        }

        public void Start()
        {
            availablePaletteNames = paletteNamesFactory.GetPaletteNames(visualization);
            config = new ClientConfig(availableFontNames, availablePaletteNames,visualization);
            var actionsDictionary = actions.ToDictionary(a => a.CommandName, a => a);
            Console.WriteLine("Список доступных комманд :");
            foreach (var action in actions)
                Console.WriteLine($"{action.CommandName}     \"{action.Description}\"");
            var userSettings = UserSettings.GetDefaultUserSettings();
            while (!config.ToExit)
            {
                Console.WriteLine("Введите команду");
                    Console.Write(">>>");
                    var command = Console.ReadLine().ToLower();
                    var commandResult = ReadCommand(command, actionsDictionary);
                    var perform = commandResult.Value.Perform(config, userSettings);
                    if(!perform.IsSuccess)
                        Console.WriteLine(perform.Error);
            }
        }

        private Result<IConsoleAction> ReadCommand(string commandName, Dictionary<string, IConsoleAction> availableCommands)
        {
            return availableCommands.ContainsKey(commandName)
                ? Result.Ok(availableCommands[commandName])
                : Result.Fail<IConsoleAction>("Unknown command");
        }
    }
}