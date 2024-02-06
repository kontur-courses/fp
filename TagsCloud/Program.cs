using McMaster.Extensions.CommandLineUtils;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;
using TagsCloud.Builders;
using TagsCloud.Entities;
using TagsCloud.Extensions;

// ReSharper disable MemberCanBePrivate.Global
namespace TagsCloud;

public class Program
{
    [Option]
    public CaseType WordsCase { get; }

    [Option]
    public bool Infinitive { get; }

    [Option("-mafs|--max-font-size")]
    public int MaxFontSize { get; } = 100;

    [Option("-mifs|--min-font-size")]
    public int MinFontSize { get; } = 30;

    [Option("-wi|--width")]
    [Required]
    public int? Width { get; }

    [Option("-he|--height")]
    [Required]
    public int? Height { get; }

    [Option(CommandOptionType.MultipleValue)]
    public IEnumerable<string> TextParts { get; } = Array.Empty<string>();

    [Option(CommandOptionType.MultipleValue)]
    public IEnumerable<string> Excluded { get; } = Array.Empty<string>();

    [Option(CommandOptionType.MultipleValue)]
    public HashSet<string> Colors { get; } = new();

    [Option]
    public bool Russian { get; } = false;

    [Option("-so|--sort")]
    public SortType Sort { get; } = SortType.Preserve;

    [Option("-me|--measurer")]
    public MeasurerType MeasurerType { get; } = MeasurerType.Linear;

    [Option]
    public ColoringStrategy Strategy { get; } = ColoringStrategy.AllRandom;

    [Option]
    public float DistanceDelta { get; } = 0.1f;

    [Option]
    public float AngleDelta { get; } = (float)Math.PI / 180;

    [Option]
    public ImageFormat OutputFormat { get; } = ImageFormat.Png;

    [Option]
    public PointGeneratorType Generator { get; } = PointGeneratorType.Spiral;

    [Option]
    public string BackgroundColor { get; } = string.Empty;

    [Option]
    public string Font { get; } = string.Empty;

    [Argument(0)]
    [Required]
    public string InputFile { get; }

    [Argument(1)]
    [Required]
    public string OutputFile { get; }

    public static int Main(string[] args)
    {
        return CommandLineApplication.Execute<Program>(args);
    }

    // ReSharper disable once UnusedMember.Local
    private void OnExecute()
    {
        var inputOptions = new InputOptionsBuilder()
                           .AsResult()
                           .Then(builder => builder.SetWordsCase(WordsCase))
                           .Then(builder => builder.SetCastPolitics(Infinitive))
                           .Then(builder => builder.SetExcludedWords(Excluded))
                           .Then(builder => builder.SetLanguageParts(TextParts))
                           .Then(builder => builder.SetLanguagePolitics(Russian))
                           .Then(builder => builder.BuildOptions())
                           .OnFail(Console.WriteLine);

        var cloudOptions = new CloudOptionsBuilder()
                           .AsResult()
                           .Then(builder => builder.SetColors(Colors))
                           .Then(builder => builder.SetLayout(
                               Generator,
                               new PointF((float)Width!.Value / 2, (float)Height!.Value / 2),
                               DistanceDelta,
                               AngleDelta))
                           .Then(builder => builder.SetColoringStrategy(Strategy))
                           .Then(builder => builder.SetMeasurerType(MeasurerType))
                           .Then(builder => builder.SetFontFamily(Font))
                           .Then(builder => builder.SetSortingType(Sort))
                           .Then(builder => builder.SetFontBounds(MinFontSize, MaxFontSize))
                           .Then(builder => builder.BuildOptions())
                           .OnFail(Console.WriteLine);

        var outputOptions = new OutputOptionsBuilder()
                            .AsResult()
                            .Then(builder => builder.SetImageFormat(OutputFormat))
                            .Then(builder => builder.SetImageSize(Width!.Value, Height!.Value))
                            .Then(builder => builder.SetImageBackgroundColor(BackgroundColor))
                            .Then(builder => builder.BuildOptions())
                            .OnFail(Console.WriteLine);

        if (!inputOptions.IsSuccess || !cloudOptions.IsSuccess || !outputOptions.IsSuccess)
            return;

        new TagCloudEngine(inputOptions.Value, cloudOptions.Value, outputOptions.Value)
            .AsResult()
            .Then(engine => engine.GenerateTagCloud(InputFile, OutputFile))
            .Then(_ => Console.WriteLine("Tag cloud image saved to file " + OutputFile))
            .OnFail(Console.WriteLine);
    }
}