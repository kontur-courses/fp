using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudContainer.Settings;
using TagsCloudContainer.TextParsers;

namespace TagsCloudContainerTests.TextParserTests
{
    [TestFixture]
    public class TextParserTests
    {
        [Test]
        public void Parse_Should_ReturnWordFrequency()
        {
            var filterSettings = A.Fake<FilterSettings>();
            var option = new Option();
            option.CountWords = 2;
            option.Converters = new string[] { };
            option.Filters = new string[] { };
            var textSettings = new TextSettings(option, filterSettings);
            var parser = new TextParser(textSettings);
            var expectation = new List<WordFrequency>
            {
                new WordFrequency("simple", 1),
                new WordFrequency("text", 1)
            };

            var result = parser.Parse($"simple{Environment.NewLine}text").Value;
            result.ShouldBeEquivalentTo(expectation);
        }
    }
}