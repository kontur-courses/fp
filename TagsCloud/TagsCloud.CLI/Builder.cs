using System.Drawing;
using System.Drawing.Imaging;
using SimpleInjector;
using TagsCloud.Core;
using TagsCloud.Core.Layouters;
using TagsCloud.Core.Painters;
using TagsCloud.Core.Settings;
using TagsCloud.Core.TagContainersProviders;
using TagsCloud.Core.TagContainersProviders.TagsPreprocessors;
using TagsCloud.Core.WordFilters;
using TagsCloud.Core.WordReaders;
using TagsCloud.Core.WordTransformers;

namespace TagsCloud.CLI;

public static class Builder
{
	public static Result<Container> Build(Options options)
	{
		var container = new Container();

		return container.AsResult()
			.Then(_ => RegisterWordsFilters(container, options))
			.Then(_ => RegisterTransformers(container))
			.Then(_ => RegisterPreprocessor(container, options))
			.Then(_ => RegisterImageSettingsProvider(container))
			.Then(_ => RegisterLayouter(container, options))
			.Then(_ => RegisterPainter(container))
			.Then(_ => RegisterContainerProvider(container))
			.Then(_ => RegisterImageSaver(container, options))
			.Then(_ => Verify(container))
			.Then(_ => container);
	}

	private static Result<Container> Verify(Container container)
	{
		return Result
			.OfAction(container.Verify)
			.ReplaceError(e => $"Can't Verify container: {e}")
			.Then(_ => container);
	}

	private static void RegisterPainter(Container container)
	{
		container.Register<ITagsCloudPainter, TagsCloudPainter>();
	}

	private static void RegisterContainerProvider(Container container)
	{
		container.Register<ITagContainersProvider, TagContainersProvider>();
	}

	private static void RegisterImageSaver(Container container, Options options)
	{
		container.RegisterInstance(typeof(ImageSaver), new ImageSaver(options.PathToImage, ImageFormat.Png));
	}

	private static Result<None> RegisterPreprocessor(Container container, Options options)
	{
		var readerResult = WordReaderFromTxt.GetReader(options.PathToWordsFile);
		if (readerResult.IsFail) return readerResult.Then(_ => new None()).RefineError("Can't read file with tags");

		container.Register<IWordReader>(() => readerResult.Value);
		container.Register<ITagsPreprocessor, TagPreprocessor>();

		return Result.Ok();
	}

	private static Result<None> RegisterWordsFilters(Container container, Options options)
	{
		var filters = new List<IWordFilter> { new MinLengthFilter(3) };

		if (options.PathToExcludedWords is not null)
		{
			var readerResult = WordReaderFromTxt.GetReader(options.PathToExcludedWords);
			if (readerResult.IsFail)
				return readerResult.Then(_ => new None()).RefineError("Can't read file with excluded words");

			filters.Add(new BoringWordsFilter(readerResult.Value));
		}

		container.Collection.Register<IWordFilter>(filters);

		container.Register<IWordFiltersComposer, WordFilterComposer>();

		return Result.Ok();
	}

	private static void RegisterImageSettingsProvider(Container container)
	{
		var imageSettingsProvider = new SettingsProvider<ImageSettings>();
		container.RegisterInstance(typeof(ISettingsSetter<ImageSettings>), imageSettingsProvider);
		container.RegisterInstance(typeof(ISettingsGetter<ImageSettings>), imageSettingsProvider);
	}

	private static void RegisterTransformers(Container container)
	{
		container.Collection.Register<IWordTransformer>(new ToLowerTransformer(0));
		container.Register<IWordTransformersComposer, WordTransformersComposer>();
	}

	private static Result<None> RegisterLayouter(Container container, Options options)
	{
		var layouterResult = options.Layouter switch
		{
			"spiral" => SpiralCloudLayouter.GetLayouter(new Point(0, 0), 100, 10),
			_ => CircularCloudLayouter.GetLayouter(new Point(0, 0))
		};

		return layouterResult
			.RefineError("Can't register layouter")
			.Then(layouter => container.RegisterInstance(typeof(ICloudLayouter), layouter));
	}
}