using System.Collections.Immutable;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using CommandLine;
using TagsCloud.Core;
using TagsCloud.Core.Painters;
using TagsCloud.Core.Painters.Pallets;
using TagsCloud.Core.Settings;
using TagsCloud.Core.TagContainersProviders;

namespace TagsCloud.CLI;

public class EntryPoint
{
	private static readonly ImmutableHashSet<string> InstalledFonts =
		new InstalledFontCollection().Families.Select(f => f.Name).ToImmutableHashSet();

	private static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			args = GetDefaultOptions();
			Console.WriteLine(
				$"Example with default options: {args.Aggregate((s1, s2) => $"{s1} {s2}")}");
		}

		Parser.Default.ParseArguments<Options>(args)
			.WithParsed(RunWithResult);
	}

	private static string[] GetDefaultOptions()
	{
		return new[]
		{
			"-w", "Numb.txt",
			"-s", "Example.png",
			"--excluding", "BoringWords.txt",
			"--font-name", "Times New Roman",
			"--use-auto-size"
		};
	}

	private static void RunWithResult(Options options)
	{
		var buildResult = Builder.Build(options);
		if (buildResult.IsFail)
		{
			Console.WriteLine($"Build error> {buildResult.ErrorMessage}");
			return;
		}

		var container = buildResult.Value;
		var settingsProvider = container.GetInstance<ISettingsSetter<ImageSettings>>();
		var containersProvider = container.GetInstance<ITagContainersProvider>();
		var painter = container.GetInstance<ITagsCloudPainter>();
		var saver = container.GetInstance<ImageSaver>();

		var runResult = GetImageSettings(options)
			.Then(settings => SendImageSettings(settings, settingsProvider))
			.Then(_ => GetContainers(containersProvider))
			.Then(tags => painter.Draw(tags))
			.Then(image => Result.OfAction(() => saver.Save(image)));

		var message = runResult.IsSuccess
			? $"OK> Image save as {options.PathToImage}"
			: $"Error> {runResult.ErrorMessage}";

		Console.WriteLine(message);
	}

	private static Result<ImageSettings> GetImageSettings(Options options)
	{
		var fontColor = Color.FromName(options.FontColor);
		if (!fontColor.IsKnownColor)
			return Result.Fail<ImageSettings>($"Unknown font color name: {options.FontColor}");

		var backgroundColor = Color.FromName(options.BackgroundColor);
		if (!backgroundColor.IsKnownColor)
			return Result.Fail<ImageSettings>($"Unknown background color name: {options.BackgroundColor}");

		ITagCLoudPallet pallet = options.UseRandomColor
			? new RandomPallet(backgroundColor)
			: new MonocolorPallet(fontColor, backgroundColor);

		if (FontNotInstalled(options.FontName))
			return Result.Fail<ImageSettings>($"Unknown font name: {options.FontName}");

		var fontFamily = new FontFamily(options.FontName);

		var fontSize = options.MinFontSize;
		if (fontSize <= 0)
			return Result.Fail<ImageSettings>($"Font size should be greater than 0, but was {fontSize}");

		var width = options.ImageWidth;
		if (width <= 0)
			return Result.Fail<ImageSettings>($"Image width should be greater than 0, but was {width}");

		var height = options.ImageHeight;
		if (height <= 0)
			return Result.Fail<ImageSettings>($"Image height should be greater than 0, but was {height}");

		return new ImageSettings
		{
			ImageSize = new Size(width, height),
			FontFamily = fontFamily,
			MinFontSize = fontSize,
			Format = ImageFormat.Png,
			Pallet = pallet,
			AutoSize = options.UseAutoSize
		};
	}

	private static void SendImageSettings(ImageSettings settings, ISettingsSetter<ImageSettings> settingsProvider)
	{
		settingsProvider.Set(settings);
	}

	private static Result<List<TagContainer>> GetContainers(ITagContainersProvider containersProvider)
	{
		var containerResults = containersProvider.GetContainers();

		return containerResults.Last().IsFail
			? Result.Fail<List<TagContainer>>($"{containerResults.Last().ErrorMessage}")
			: containerResults.Select(result => result.Value).ToList().AsResult();
	}

	private static bool FontNotInstalled(string fontName)
	{
		return !InstalledFonts.Contains(fontName);
	}
}