using Autofac;
using CommandLine;
using ResultOf;
using TagsCloudVisualization.CloudDrawer;
using TagsCloudVisualization.TextInput;

namespace TagsCloudVisualization.Clients;

public class ConsoleClient
{
    private Options options;

    public ConsoleClient(params string[] args)
    {
        options = Parser.Default.ParseArguments<Options>(args).Value;
    }

    public void Run()
    {
        AppContainer.Configure(options);

        using (var scope = AppContainer.GetScope())
        {
            var textInput = scope.Resolve<ITextInput>();
            var generator = scope.Resolve<ICloudGenerator>();
            var drawer = scope.Resolve<ICloudDrawer>();

            textInput.GetInputString()
                .Then(generator.GenerateCloud)
                .Then(drawer.Draw);
        }
    }
}