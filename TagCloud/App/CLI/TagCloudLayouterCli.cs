using System;
using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Graphics;
using TagCloud.Infrastructure.Settings;
using TagCloud.Infrastructure.Settings.UISettingsManagers;

namespace TagCloud.App.CLI
{
    public class TagCloudLayouterCli : IApp
    {
        private readonly Func<Settings> settingsFactory;
        private readonly IEnumerable<IInputManager> settingsManagers;
        private readonly IImageGenerator generator;
        private readonly ImageSaver imageSaver;
        
        private readonly IIOBridge bridge;

        public TagCloudLayouterCli(Func<Settings> settingsFactory, IEnumerable<IInputManager> settingsManagers, IImageGenerator generator, ImageSaver imageSaver, IIOBridge bridge)
        {
            this.settingsFactory = settingsFactory;
            this.settingsManagers = settingsManagers;
            this.generator = generator;
            this.imageSaver = imageSaver;
            this.bridge = bridge;
        }

        public void Run()
        {
            var automata = new Automata();

            var mainState = new State("Main");
            mainState.Show += DisplayState;
            mainState.Act += OnMainInput;
            var helpState = new State("Help");
            helpState.Show += DisplayState;
            helpState.Show += OnHelp;
            var exitState = new State(true);
            exitState.Show += OnExit;
            var aboutState = new State("About");
            aboutState.Show += OnAbout;
            var settingsState = new State("Settings");
            settingsState.Show += DisplayState;
            settingsState.Show += OnSettingsState;

            automata.Init(mainState);
            automata.Add(new Transition(mainState, "help", helpState));
            automata.Add(new Transition(mainState, "exit", exitState));
            automata.Add(new Transition(helpState, "about", aboutState));
            automata.Add(new Transition(helpState, @".*", mainState));
            automata.Add(new Transition(aboutState, ".*", helpState));

            automata.Add(new Transition(mainState, "set", settingsState));
            automata.Add(new Transition(mainState, "settings", settingsState));
            var managersStates = GetSettingsManagersStates(settingsManagers);
            AddSettingsManagersTransitions(automata, settingsState, managersStates);
            automata.Add(new Transition(settingsState, @"\D*", mainState));

            var generateState = new State("Generation");
            generateState.Show += OnGenerateState;
            automata.Add(new Transition(mainState, "gen", generateState));
            automata.Add(new Transition(mainState, "generate", generateState));
            automata.Add(new Transition(generateState, ".*", mainState));

            settingsFactory().Import(Program.GetDefaultSettings());

            while (automata.Show())
            {
                bridge.Write("> ");
                var inp = bridge.Read();
                bridge.Next();
                automata.Move(inp);
            }
        }

        private void OnGenerateState(State sender, EventArgs args)
        {
            using var image = generator.Generate();
            bridge.WriteLine("Layout ready");
            var result = imageSaver.Save(image);
            bridge.WriteLine(result.IsSuccess ? result.Value : result.Error);
        }

        private void AddSettingsManagersTransitions(Automata automata, State from, IEnumerable<State> states)
        {
            foreach (var (state, i) in states.Select((state, i) => (state, i)))
            {
                automata.Add(new Transition(from, $"{i}", state));
                automata.Add(new Transition(state, ".*", from));
            }
        }

        private IEnumerable<State> GetSettingsManagersStates(IEnumerable<IInputManager> managers)
        {
            foreach (var manager in managers)
            {
                var state = new State(manager.Title);
                state.Show += (sender, args) =>
                {
                    bridge.WriteLine(
                        $"CHANGING\n\t{manager.Title}\ninfo\t{manager.Help}\nvalue\t{manager.Get()}\ninput new value");
                };
                state.Act += (sender, args) =>
                {
                    // todo TextWriter instead console
                    var result = manager.TrySet(args.Input);
                    bridge.WriteLine(!result.IsSuccess
                        ? result.Error
                        : $"{manager.Title} was changed to '{result.Value}'");
                };
                yield return state;
            }
        }

        private void OnSettingsState(State sender, EventArgs args)
        {
            foreach (var (manager, i) in settingsManagers.Select((manager, i) => (manager, i)))
                bridge.WriteLine($"{i})\t{manager.Title}\ninfo\t{manager.Help}\nvalue\t{manager.Get()}\n");
            bridge.WriteLine("Choose setting number ");
        }

        private void OnAbout(State sender, EventArgs args)
        {
            bridge.WriteLine("I\nAM\nRUS\nLAND");
        }

        private void OnHelp(object sender, EventArgs args)
        {
            bridge.WriteLine("Available commands in main");
            bridge.WriteLine("\thelp");
            bridge.WriteLine("\texit");
            bridge.WriteLine("\tsettings");
            bridge.WriteLine("\tgenerate");
            bridge.WriteLine("press Enter key to go back");
        }

        private void OnExit(State state, EventArgs eventArgs)
        {
            bridge.WriteLine("Bye bye");
        }

        private void DisplayState(State sender, EventArgs args)
        {
            bridge.WriteLine($"$ {sender.Name}:");
        }

        private void OnMainInput(object sender, ConsoleInputEventArgs args)
        {
            if (!args.IsTransfer)
                bridge.WriteLine($"'{args.Input}' not found. Type help for help");
        }
    }
}