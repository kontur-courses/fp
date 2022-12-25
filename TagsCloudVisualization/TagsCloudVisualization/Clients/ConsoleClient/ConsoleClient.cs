using Autofac;
using CommandLine;
using java.lang;
using ResultOf;
using TagsCloudVisualization.CloudDrawer;
using TagsCloudVisualization.TextInput;

namespace TagsCloudVisualization.Clients;

public class ConsoleClient
{
    public void Run(params string[] args)
    {
        var configuration = Result
            .Of(() => Parse(args))
            .RefineError("Options error")
            .Then(AppContainer.Configure)
            .RefineError("Configuration error")
            .OnFail(PrintError);

        if (!configuration.IsSuccess)
            return;

        using (var scope = AppContainer.GetScope())
        {
            var textInput = scope.Resolve<ITextInput>();
            var generator = scope.Resolve<ICloudGenerator>();
            var drawer = scope.Resolve<ICloudDrawer>();

            textInput.GetInputString()
                .RefineError("Input error")
                .Then(generator.GenerateCloud)
                .RefineError("Generation error")
                .Then(drawer.Draw)
                .RefineError("Drawing error")
                .OnFail(PrintError);
        }
    }

    private void PrintError(string error) => Console.Write(error);

    private Options Parse(string[] args)
    {
        var val = Parser.Default.ParseArguments<Options>(args).Value;
        if (val is null)
            throw new System.Exception("Invalid option");
        return val;
    }
}