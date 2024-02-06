using Autofac;
using CommandLine;
using FakeItEasy;
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
    private IFileReader reader;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        reader = A.Fake<IFileReader>();
        A.CallTo(() => reader.ReadLines(A<string>.Ignored))
            .Returns(Result.Ok(Enumerable.AsEnumerable(new List<string>() { "test" })));
        A.CallTo(() => reader.GetAvailableExtensions()).Returns(new List<string>() { "txt" });

        settings = Parser.Default.ParseArguments<Settings>(new List<string>()).Value;

        var builder = Configurator.ConfigureBuilder(settings);
        builder.Register(c => reader).As<IFileReader>();
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
}