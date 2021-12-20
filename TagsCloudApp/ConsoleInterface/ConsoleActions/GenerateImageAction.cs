using TagsCloudContainer.Appliers;
using TagsCloudContainer.BitmapSavers;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Parsers;
using TagsCloudContainer.TagCloudPainters;
using TagsCloudContainer.TagCreators;
using TagsCloudContainer.TagPainters;

namespace TagsCloudApp.ConsoleInterface.ConsoleActions;

public class GenerateImageAction : IUIAction
{
    private readonly IFiltersApplier filtersApplier;
    private readonly Dictionary<string, IParser> parserSelector;
    private readonly IPreprocessorsApplier preprocessorsApplier;
    private readonly ITagCreator tagCreator;
    private readonly ITagPainter tagPainter;
    private readonly ICloudTagCreator cloudTagCreator;
    private readonly ITagCloudPainter cloudPainter;
    private readonly IBitmapSaver bitmapSaver;

    public GenerateImageAction(
        ITagCloudPainter cloudPainter,
        IFiltersApplier filtersApplier, 
        ITagCreator tagCreator,
        CloudSettings cloudSettings, 
        IParser[] parsers,
        IPreprocessorsApplier preprocessorsApplier,
        ICloudTagCreator cloudTagCreator,
        IBitmapSaver bitmapSaver)
    {
        this.preprocessorsApplier = preprocessorsApplier;
        this.filtersApplier = filtersApplier;
        tagPainter = cloudSettings.Painter;
        this.tagCreator = tagCreator;
        this.cloudTagCreator = cloudTagCreator;
        this.cloudPainter = cloudPainter;
        this.bitmapSaver = bitmapSaver;
        parserSelector = new Dictionary<string, IParser>();
        for (var i = 1; i <= parsers.Length; i++)
        {
            var available = parsers[i - 1];
            foreach (var format in available.GetFormats())
                parserSelector[format] = available;
        }
    }

    public string GetDescription()
    {
        return "Generate image";
    }

    public void Handle()
    {
        var textsPath = Path.GetFullPath(@"..\..\..\texts");

        Console.WriteLine($"Texts path is {textsPath}");
        Console.WriteLine("Enter file from this folder as");
        Console.WriteLine("NAME.FORMAT");
        Console.WriteLine("Or pass an empty string to be brought back to menu");
        Console.WriteLine("Supported formats are");
        Console.WriteLine(string.Join(',', parserSelector.Keys.ToArray()));
        var answer = Console.ReadLine() ?? "";
        Console.WriteLine();
        var result = HandleInput(answer);
        if (result.IsSuccess)
            PaintSource(Path.Combine(textsPath, answer), result.Value);
        else
            Console.WriteLine(result.Error);
    }

    private Result<IParser> HandleInput(string answer)
    {
        var split = answer.Split('.');

        if (split.Length != 2)
            return Result.Fail<IParser>("Incorrect input!");

        var format = split[1];
        if (!parserSelector.ContainsKey(format))
            return Result.Fail<IParser>("File format is not supported!");
        return Result.Ok(parserSelector[format]);
    }

    private void PaintSource(string sourcePath, IParser parser)
    {
        var result = GenerateImage(sourcePath, parser);
        if (result.IsSuccess)
        {
            Console.WriteLine("The path for image");
            Console.WriteLine(result.Value);
        }
        else
        {
            Console.WriteLine(result.Error);
        }

        Console.WriteLine();
    }

    private Result<string> GenerateImage(string path, IParser parser)
    {
        return parser.Parse(path)
            .Then(preprocessorsApplier.ApplyPreprocessors)
            .Then(filtersApplier.ApplyFilters)
            .Then(tagCreator.CreateTags)
            .Then(tagPainter.PaintTags)
            .Then(cloudTagCreator.CreateCloudTags)
            .Then(cloudPainter.Paint)
            .Then(bitmapSaver.SaveBitmap);
    }
}