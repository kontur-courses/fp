using Autofac;
using CommandLine;
using ResultOf;
using TagCloud.AppSettings;
using TagCloud.FileReader;
using TagCloud.UserInterface;

namespace TagCloudTests;

[TestFixture]
public class ConsoleUI_Should
{
    private IAppSettings settings;
    private IUserInterface sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        settings = Parser.Default.ParseArguments<Settings>(new List<string>()).Value;

        var builder = Configurator.ConfigureBuilder(settings);

        builder.RegisterType<FakeReader>().As<IFileReader>();

        var container = builder.Build();
        sut = container.Resolve<IUserInterface>();
    }

    [Test]
    public void GenerateFileWithCorrectSettings()
    {
        sut.Run(settings);

        File.Exists($"{settings.OutputPath}.{settings.ImageExtension}").Should().BeTrue();

        File.Delete($"{settings.OutputPath}.{settings.ImageExtension}");
    }

    private class FakeReader : IFileReader
    {
        public Result<IEnumerable<string>> ReadLines(string inputPath)
        {
            return Result.Ok(GetText());
        }

        private IEnumerable<string> GetText()
        {
            yield return "test";
        }

        public IEnumerable<string> GetAvailableExtensions()
        {
            return new List<string>() { "txt" };
        }
    }
}