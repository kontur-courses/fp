using ConsoleApp.Options;
using MyStemWrapper;
using TagsCloudContainer;
using TagsCloudContainer.Settings;

namespace ConsoleApp.Handlers;

public class GenerateCloudOptionsHandler : IOptionsHandler
{
    private readonly MyStem myStem;
    private readonly IAppSettings appSettings;
    private readonly IAnalyseSettings analyseSettings;
    private readonly ITagsCloudContainer cloudContainer;

    public GenerateCloudOptionsHandler(IAppSettings appSettings, MyStem myStem, IAnalyseSettings analyseSettings,
        ITagsCloudContainer cloudContainer)
    {
        this.appSettings = appSettings;
        this.myStem = myStem;
        this.analyseSettings = analyseSettings;
        this.cloudContainer = cloudContainer;
    }

    public bool CanParse(IOptions options)
    {
        return options is GenerateCloudOptions;
    }

    public Result<string> ProcessOptions(IOptions options, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (options is not GenerateCloudOptions opts)
            return Result.Fail<string>("Не удалось определить параметры генерации.");
        Map(opts);
        return Execute();
    }

    private void Map(GenerateCloudOptions options)
    {
        appSettings.InputFile = options.InputFile;
        appSettings.OutputFile = options.OutputFile;

        if (!string.IsNullOrWhiteSpace(options.AnalyseParameters))
            myStem.Parameters = "-" + options.AnalyseParameters;
        if (options.ValidSpeechParts.Any())
            analyseSettings.ValidSpeechParts = options.ValidSpeechParts.ToArray();
    }

    private string Execute()
    {
        var result = cloudContainer.GenerateImageToFile(appSettings.InputFile, appSettings.OutputFile);
        return result.IsSuccess
            ? $"Успешно сохранено в файл - \"{appSettings.OutputFile}\"."
            : result.Error;
    }
}