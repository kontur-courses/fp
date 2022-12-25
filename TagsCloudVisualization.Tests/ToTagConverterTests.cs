using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.ToTagConverter;

namespace TagsCloudVisualization.Tests;

[TestFixture]
public class ToTagConverterTests
{
    private WordToTagConverter converter;

    [SetUp]
    public void Setup()
    {
        converter = new WordToTagConverter();
    }

    [Test]
    public void Convert_ShouldReturnEmptyTags_WhenGetEmptyCollection()
    {
        var words = Array.Empty<string>();

        var tags = converter.Convert(words);

        tags.GetValueOrThrow().Should().BeEmpty();
    }

    [Test]
    public void Convert_ShouldReturnTagsWithDescendingWeight()
    {
        var words = new[]
        {
            "Java",
            "python",
            "python",
            "C#",
            "C#",
            "C#"
        };

        var tags = converter.Convert(words).GetValueOrThrow().ToArray();

        tags.Should().HaveCount(3);
        tags.Select(x => x.Weight)
            .Should()
            .BeInDescendingOrder();
    }
}