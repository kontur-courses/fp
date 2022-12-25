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
        var optionsResult = Result
            .Of(() => Parser.Default.ParseArguments<Options>(args).Value)
            .RefineError("Options error")
            .OnFail(e => Console.Write(e));
        if (!optionsResult.IsSuccess)
            return;

        AppContainer.Configure(optionsResult.Value);
        using (var scope = AppContainer.GetScope())
        {
            var textInput = scope.Resolve<ITextInput>();
            var generator = scope.Resolve<ICloudGenerator>();
            var drawer = scope.Resolve<ICloudDrawer>();

            textInput.GetInputString()
                .Then(generator.GenerateCloud)
                .Then(drawer.Draw)
                .OnFail(error => Console.Write(error));
        }
    }
}