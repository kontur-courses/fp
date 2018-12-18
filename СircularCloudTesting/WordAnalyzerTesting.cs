using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NHunspell;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.WordProcessing;

namespace СircularCloudTesting
{
    [TestFixture]
    public class WordAnalyzerTesting
    {
        private Dictionary<string, int> expectedResult;

        [SetUp]
        public void Init()
        {
            expectedResult = expectedResult = new Dictionary<string, int>
            {
                {"вырезать",1 },{"ракета",1 }, {"картон",1 },{"строгий",1 },
                {"тон",1 }, {"произнёс",1},{"вези",1 },{"меня",1 }, {"сидней",3 }
            };
        }

        [TestCase("testDocFile.doc", TestName = "File has an extension doc")]
        [TestCase("testDocxFile.docx", TestName = "File has an extension docx")]
        [TestCase("testTxtFile.txt", TestName = "File has an extension txt")]
        public void MakeWordFrequencyDictionary_Should_ProcessFileCorrectly_When(string fileName)
        {
            var wordAnalyzer = new WordAnalyzer(new WordsSettings()
            { PathToFile = $"{AppDomain.CurrentDomain.BaseDirectory}/TestingFiles/" + fileName });
            var result = wordAnalyzer.MakeWordFrequencyDictionary();
            result.GetValueOrThrow().Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void MakeWordFrequencyDictionary_Should_Fail_WhenHanspellCanNotFindDictionaries()
        {
            var wordAnalyzer = new WordAnalyzer(new WordsSettings()
            { PathToFile = $"{AppDomain.CurrentDomain.BaseDirectory}/TestingFiles/testTxtFile.txt" });
            var field = typeof(WordAnalyzer)
                .GetField("hunspell", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(wordAnalyzer, Result.Of(() => new Hunspell(@"D:\dict.aff", @"C:\dict.dic")));
            var result = wordAnalyzer.MakeWordFrequencyDictionary();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(@"One of the external libraries failed. AFF File not found: D:\dict.aff");
        }

        [Test]
        public void MakeWordFrequencyDictionary_Should_WhenFailReadTxtFile()
        {
            var wordAnalyzer = new WordAnalyzer(new WordsSettings()
            { PathToFile = @"D:\badFile.txt" });
            var result = wordAnalyzer.MakeWordFrequencyDictionary();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(@"The settings file could not be read. Could not read txt file.");
        }

        [Test]
        public void MakeWordFrequencyDictionary_Should_WhenFailReadDocFile()
        {
            var wordAnalyzer = new WordAnalyzer(new WordsSettings()
            { PathToFile = @"D:\badFile.doc" });
            var result = wordAnalyzer.MakeWordFrequencyDictionary();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("The settings file could not be read. One of the external" +
                                                 " libraries failed. Could not read doc or docx file.");
        }

        [Test]
        public void MakeWordFrequencyDictionary_Should_WhenFailReadFile()
        {
            var wordAnalyzer = new WordAnalyzer(new WordsSettings()
            { PathToFile = @"D:\badFile.rtf" });
            var result = wordAnalyzer.MakeWordFrequencyDictionary();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("The settings file could not be read. Could not read file.");
        }
    }
}