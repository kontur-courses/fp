using FluentAssertions;
using System.Drawing;
using System.Text;
using TagCloudContainer.BoringFilters;
using TagCloudContainer.Formatters;
using TagCloudContainer.FrequencyWords;
using TagCloudContainer.Models;
using TagCloudContainer.Parsers;
using TagCloudContainer.Readers;
using TagCloudContainer.TagsWithFont;

namespace TagCloudTests
{
    public class Tests
    {
        [Test]
        public void ReaderShould_ParseTextCorrectly()
        {
            var parser = new FileLinesParser();
            var testString = @"hello
super
word";
            var parsedText = new FileLinesParser().Parse(testString);
            parsedText.Value.Count().Should().Be(3);

            var testStringSplitted = testString.Split('\n').Select(s => s.Trim()).ToList();

            for (var i = 0; i < testStringSplitted.Count(); i++)
                parsedText.Value.ToList()[i].Should().Be(testStringSplitted[i]);
        }

        [Test]
        public void ReaderShould_ParseTextCorrectly_WhenBoringFlagEnabled()
        {
            var parser = new FileLinesParser();
            var testString = @"привет
это
тест
с
подвохом";
            var parsedText = new FileLinesParser().Parse(testString);
            var filteredText = new BoringFilter().FilterWords(parsedText.Value);
            filteredText.Value.Count().Should().Be(3);
        }

        [Test]
        public void ReaderShould_ReadDocxCorrectly()
        {
            var reader = new Reader();
          
            var text = reader.ReadFile($"test_reader.docx");
            text.IsSuccess.Should().Be(true);
            text.Value.Trim().Should().Be("test" + Environment.NewLine + "reader");
        }

        [Test]
        public void ReaderShould_ReadTxtCorrectly()
        {
            var reader = new Reader();

            var text = reader.ReadFile($"test_reader.txt");

            text.IsSuccess.Should().Be(true);
            text.Value.Trim().Should().Be("test" + Environment.NewLine + "reader");
        }

        [Test]
        public void FrequencyTagsShould_CountCorrectly()
        {
            var words = new List<string>()
            {
                "a",
                "aaa", "aaa",
                "b", "b", "b", "b"
            };

            var wordsFrequency = new FrequencyCounter().GetTagsFrequency(words).Value.ToList();
            wordsFrequency.Count().Should().Be(3);

            wordsFrequency[0].Count.Should().Be(4);
            wordsFrequency[1].Count.Should().Be(2);
            wordsFrequency[2].Count.Should().Be(1);
        }

        [Test]
        public void WordFromatterShould_ApplyFunctionForEachelement()
        {
            var words = new List<string>()
            {
                "a",
                "aaa", "aaa",
                "b", "b", "b", "b"
            };

            var result = new WordFormatter().Normalize(words, s => s.ToUpper()).Value.ToArray();

            for (var i = 0; i < words.Count(); i++)
                words[i].ToUpper().Should().Be(result[i]);
        }

        [Test]
        public void FontSizerShould_CalculateSizes()
        {
            var fontSizer = new FontSizer();
            var words = new List<string>()
            {
                "a", "bb", "bb"
            };
           
            var fontTags = fontSizer
                .GetTagsWithSize(
                new FrequencyCounter().GetTagsFrequency(words).Value,
                new FontSettings() { MaxFontSize = 150, MinFontSize = 50, Font = new FontFamily("Arial") }
                ).Value.ToList();

            fontTags.Count.Should().Be(2);

            fontTags[0].SizeFont.Should().Be(150);
            fontTags[1].SizeFont.Should().Be(50);
        }
    }
}