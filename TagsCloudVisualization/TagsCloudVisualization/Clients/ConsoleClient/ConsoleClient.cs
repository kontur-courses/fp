using Autofac;
using CommandLine;
using ResultOf;
using TagsCloudVisualization.CloudDrawer;
using TagsCloudVisualization.TextInput;

namespace TagsCloudVisualization.Clients;

public class ConsoleClient
{
    public void Run(params string[] args)
    {
        Result
            .Of(() => Parser.Default.ParseArguments<Options>(args).Value)
            .RefineError("Options error")
            .OnFail(WriteError)
            .Then(AppContainer.Configure)
            .RefineError("Configuration error")
            .OnFail(WriteError);

        using (var scope = AppContainer.GetScope())
        {
            var textInput = scope.Resolve<ITextInput>();
            var generator = scope.Resolve<ICloudGenerator>();
            var drawer = scope.Resolve<ICloudDrawer>();

            textInput.GetInputString()
                .Then(generator.GenerateCloud)
                .RefineError("Generation error")
                .OnFail(WriteError)
                .Then(drawer.Draw)
                .RefineError("Drawing error")
                .OnFail(WriteError);
        }
    }

    private void WriteError(string error) => Console.Write(error);
}