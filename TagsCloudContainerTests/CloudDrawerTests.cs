using CloudLayout;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Interfaces;
using ResultOfTask;

namespace TagsCloudContainerTests;

public class CloudDrawer_Should
{
    private string firstPath;
    private string secondPath;
    private ICustomOptions defaultOptions;
    private CloudDrawer sut;


    [SetUp]
    public void SetUpDrawer()
    {
        var converter = new FileToDictionaryConverter(new WordsFilter(), new BudgetDocParser());
        var calculator = new WordSizeCalculator();
        var spiralDrawer = new SpiralDrawer();
        sut = new CloudDrawer(spiralDrawer, converter, calculator);

        defaultOptions = GetDeafultResultOptions();
        firstPath = Path.Combine(defaultOptions.WorkingDirectory,
            (defaultOptions.ImageName + "1." + defaultOptions.ImageFormat));
        secondPath = Path.Combine(defaultOptions.WorkingDirectory,
            (defaultOptions.ImageName + "2." + defaultOptions.ImageFormat));
    }

    [TearDown]
    public void CleanUp()
    {
        File.Delete(firstPath);
        File.Delete(secondPath);

        defaultOptions = GetDeafultResultOptions();
        firstPath = Path.Combine(defaultOptions.WorkingDirectory,
            (defaultOptions.ImageName + "1." + defaultOptions.ImageFormat));
        secondPath = Path.Combine(defaultOptions.WorkingDirectory,
            (defaultOptions.ImageName + "2." + defaultOptions.ImageFormat));
    }

    private ICustomOptions GetDeafultResultOptions()
    {
        return new CustomOptions
        {
            WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\WorkingDir"),
            WordsFileName = "FiveSingleWords.txt",
            BoringWordsName = "EmptyList.txt",
            Font = "Arial",
            PictureSize = 600,
            MinTagSize = 15,
            MaxTagSize = 35,
            BackgroundColor = "White",
            FontColor = "Blue",
            ExcludedParticals = "SPRO, PR, PART, CONJ",
            ImageFormat = "Png",
            ImageName = "Cloud1"
        };
    }

    [Test]
    public void DrawAPicture()
    {
        sut.DrawCloud(firstPath, defaultOptions);

        File.Exists(firstPath).Should().BeTrue();
    }

    [TestCase("png")]
    [TestCase("jpeg")]
    public void DrawAPNGPicture(string format)
    {
        defaultOptions.ImageFormat = format;
        sut.DrawCloud(firstPath, defaultOptions);
        firstPath = Path.Combine(defaultOptions.WorkingDirectory,
            (defaultOptions.ImageName + "1." + defaultOptions.ImageFormat));

        var result = new FileInfo(firstPath);

        result.Extension.Should().Be("." + format);
    }

    [Test]
    public void DrawSamePictureFromDocAndTxtWithSameWordsInThem()
    {
        var docOptions = (CustomOptions)((CustomOptions)defaultOptions).Clone();
        docOptions.WordsFileName = "SmallText.doc";
        defaultOptions.WordsFileName = "SmallText.txt";

        sut.DrawCloud(firstPath, defaultOptions);
        sut.DrawCloud(secondPath, docOptions);

        var file1 = new FileInfo(firstPath);
        var file2 = new FileInfo(secondPath);
        file1.Length.Should().Be(file2.Length);
    }

    [Test]
    [Description("Sizes are relative to word count")]
    public void DrawSamePictureFromFiveSingleWordsAndFiveWordsPair()
    {
        var fivePairsOption = (CustomOptions)((CustomOptions)defaultOptions).Clone();
        fivePairsOption.WordsFileName = "FiveWordsPair.txt";
        defaultOptions.WordsFileName = "FiveSingleWords.txt";

        sut.DrawCloud(firstPath, defaultOptions);
        sut.DrawCloud(secondPath, fivePairsOption);

        var file1 = new FileInfo(firstPath);
        var file2 = new FileInfo(secondPath);
        file1.Length.Should().Be(file2.Length);
    }

    [Test]
    public void DrawBiggerPictureWithoutBoringWordsList()
    {
        var boringWordsOption = (CustomOptions)((CustomOptions)defaultOptions).Clone();
        boringWordsOption.BoringWordsName = "SomeBoringWords.txt";

        sut.DrawCloud(firstPath, defaultOptions);
        sut.DrawCloud(secondPath, boringWordsOption);

        var file1 = new FileInfo(firstPath);
        var file2 = new FileInfo(secondPath);
        file1.Length.Should().BeGreaterThan(file2.Length);
    }
}