using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.Infrastructure.FileReader;
using TagCloud.Infrastructure.Filter;
using TagCloud.Infrastructure.Layouter;
using TagCloud.Infrastructure.Lemmatizer;
using TagCloud.Infrastructure.Monad;
using TagCloud.Infrastructure.Painter;
using TagCloud.Infrastructure.Pipeline;
using TagCloud.Infrastructure.Pipeline.Common;
using TagCloud.Infrastructure.Saver;
using TagCloud.Infrastructure.Weigher;

namespace TagCloudTests.Infrastructure.Pipeline;

internal class ImagePipelineTests
{
    private IAppSettings settings;
    private IImagePipeline sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        settings = new AppSettings { OutputPath = "outputTest" };
        var layouter = new CircularCloudLayouter(settings);
        var factory = new CloudLayouterFactory(new ICloudLayouter[] { layouter }, layouter);
        var painter = new Painter(new RandomPalette(), factory, settings);
        sut = new ImageProcessor(new FakeFileReaderFactory(), painter, new WordWeigher(), new ImageSaver(), new RussianLemmatizer(), new Filter());
    }

    [Test]
    public void Run_TransferImageFileToSaver()
    {
        var filepath = $"{Environment.CurrentDirectory}\\{settings.OutputPath}.{settings.OutputFormat}";

        if (File.Exists(filepath))
            File.Delete(filepath);

        sut.Run(settings);

        File.Exists(filepath).Should().BeTrue();
    }

    private class FakeFileReaderFactory : IFileReaderFactory
    {
        public Result<IFileReader> Create(string filePath)
        {
            return Result.Ok<IFileReader>(new FakeReader());
        }
    }

    private class FakeReader : IFileReader
    {
        public Result<IEnumerable<string>> GetLines(string inputPath)
        {
            return Result.Ok((IEnumerable<string>) new List<string> { "test" });
        }

        public IEnumerable<string> GetSupportedExtensions()
        {
            throw new NotImplementedException(); //ignored
        }
    }
}