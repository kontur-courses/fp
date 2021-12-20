using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TagsCloudVisualizationDI;
using TagsCloudVisualizationDI.AnalyzedTextReader;
using TagsCloudVisualizationDI.TextAnalyze;
using TagsCloudVisualizationDI.TextAnalyze.Analyzer;

namespace TagsCloudVisualizationDITests
{
    [TestFixture]
    public class AnalyzerTests
    {
        private const string Arguments = "-lndw -ig";
        private static readonly PartsOfSpeech.SpeechPart[] ExcludedSpeechParts = new[]
        {
            PartsOfSpeech.SpeechPart.CONJ, PartsOfSpeech.SpeechPart.INTJ,
            PartsOfSpeech.SpeechPart.PART, PartsOfSpeech.SpeechPart.PR,
        };
        private static readonly string MyStemPath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\mystem.exe";

        [Test]
        public void ShouldNotThrowWhenPathsAreValid()
        {
            var path = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\ex2.TXT";
            var savePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\result.TXT";
            var analyzer = new DefaultAnalyzer(ExcludedSpeechParts, new List<string>(), path, 
                savePath, MyStemPath, Arguments) as IAnalyzer;
            Action invoking = () => analyzer.InvokeMystemAnalizationResult().GetValueOrThrow();
            invoking.Should().NotThrow();

        }

        [Test]
        public void ShouldGetRightAnalyzedWords()
        {
            var path = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\ex2.TXT";
            var savePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + "\\result.TXT";

            var analyzer = new DefaultAnalyzer(ExcludedSpeechParts, new List<string>(), path,
                savePath, MyStemPath, Arguments) as IAnalyzer;
            var reader = new DefaultAnalyzedTextFileReader(savePath, Encoding.UTF8) as IAnalyzedTextFileReader;
            analyzer.InvokeMystemAnalizationResult();
            var words = reader.ReadText();

            var result = analyzer.GetAnalyzedWords(words.GetValueOrThrow()).ToList();
            var expectedResult = new List<Word>
            {
                new Word("тот"),
                new Word("ураган"),
                new Word("проходить"),
                new Word("мы"),
                new Word("мало"),
                new Word("уцелеть"),
            };
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
