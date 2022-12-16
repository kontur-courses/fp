using System.Drawing;
using NUnit.Framework;
using FluentAssertions;
using TagCloud;
using TagsCloudLayouter;

namespace TagCloudTests;

[TestFixture]
public class TagCloudConstructorTests
{
    private ApplicationProperties properties;
    private ICloudDrawer drawer;
    private FrequencyDictionary frequencyDictionary;
    private ICloudLayouter layouter;
    private IWordsParser parser;
    private SizeByFrequency sizeByFrequency;
    private IFileLoader textLoader;
    private IWordPreprocessor wordPreprocessor;
    
    [SetUp]
    public void SetUp()
    {
        properties = new ApplicationProperties
        {
            SizeProperties = new SizeProperties { ImageSize = new Size(14048, 14048) },
            CloudProperties = new CloudProperties(),
            FontProperties = new FontProperties(
                10f, 
                64F, 
                FontStyle.Regular,
                ContentAlignment.MiddleCenter, 
                FontFamily.GenericSansSerif),
            Palette = new Palette(Color.Black, Color.White),
            Path = "Words.txt"
        };
        sizeByFrequency = new SizeByFrequency(properties.FontProperties);
        drawer = new CloudDrawer(properties.SizeProperties, properties.Palette);
        layouter = new CircularCloudLayouter(new Point());
        frequencyDictionary = new FrequencyDictionary();
        parser = new WordsParser();
        textLoader = new TxtFileLoader();
        wordPreprocessor = new WordPreprocessor();
    }

    [Test]
    public void Correct_OnWordsAreInBound()
    {
        var constructor = new TagCloudConstructor(
            drawer, 
            textLoader, 
            properties, 
            parser, 
            sizeByFrequency, 
            layouter,
            wordPreprocessor, 
            frequencyDictionary);
        
        var result = constructor.Construct();
        result.IsSuccess.Should().BeTrue();
    }
    
    [Test]
    public void Error_OnWordsAreNotInBound()
    {
        var constructor = new TagCloudConstructor(
            drawer, 
            textLoader, 
            properties, 
            parser, 
            sizeByFrequency, 
            layouter,
            wordPreprocessor, 
            frequencyDictionary);
        properties.SizeProperties.ImageSize = new Size(512, 512);
        var result = constructor.Construct();
        result.IsSuccess.Should().BeFalse();
    }
}