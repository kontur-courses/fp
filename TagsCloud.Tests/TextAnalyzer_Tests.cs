using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Core;

namespace TagsCloud.Tests
{
    internal class TextAnalyzer_Tests
    {
        private static readonly PathSettings PathSettings
            = new PathSettings
            {
                PathToAffix = "Texts/ru_RU.aff", 
                PathToDictionary = "Texts/ru_RU.dic"
            };

        private readonly TextAnalyzer textAnalyzer = new TextAnalyzer(new HunspellFactory(), PathSettings);

        [Test]
        public void GetWordByFrequency_CorrectFrequencyWithSortingByDescending()
        {
            var text = "баян\nрусское\nрусский\nрусского\nбани\nконь\nконя\nбанный";
            var expected = new List<(string, int)>
            {
                ("русский", 3),
                ("конь", 2),
                ("банный", 1),
                ("баня", 1),
                ("баян", 1)
            };

            var actual = textAnalyzer.GetWordByFrequency(
                text.Split('\n').ToList(),
                x => x.OrderByDescending(y => y.Value)
                    .ThenByDescending(y => y.Key.Length)
                    .ThenBy(y => y.Key, StringComparer.Ordinal))
                .GetValueOrThrow();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetWordByFrequency_CorrectFrequencyWithSortingByAscending()
        {
            var text = "баян\nрусское\nрусский\nрусского\nбани\nконь\nконя\nбанный";
            var expected = new List<(string, int)>
            {
                ("баня", 1),
                ("баян", 1),
                ("банный", 1),
                ("конь", 2),
                ("русский", 3)
            };

            var actual = textAnalyzer.GetWordByFrequency(
                    text.Split('\n').ToList(),
                    x => x.OrderBy(y => y.Value)
                        .ThenByDescending(y => y.Key.Length)
                        .ThenBy(y => y.Key, StringComparer.Ordinal))
                .GetValueOrThrow();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}