using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using TagsCloudVisualization.Words;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class CustomWordSizeCalculatorTests
{
    private IWordsSizeCalculator _wordsSizeCalculator;

    [SetUp]
    public void Setup()
    {
        _wordsSizeCalculator = new CustomWordSizeCalculator();
    }

    [TestCase(20, 10, Description = "MaxFontSize больше чем MinFontSize")]
    [TestCase(25, 25, Description = "MaxFontSize равен MinFontSize")]
    public void CalcSizeForWords_MaxFontSizeAndMinFontSize_IsNotSuccess(float minFontSize, float maxFontSize)
    {
        _wordsSizeCalculator.CalcSizeForWords(new Dictionary<string, int>(), minFontSize, maxFontSize).IsSuccess
            .Should()
            .BeFalse();
    }

    [Test]
    public void CalcSizeForWords_MinFontSizeLessThanOne_IsNotSuccess()
    {
        _wordsSizeCalculator.CalcSizeForWords(new Dictionary<string, int>(), 0, 20).IsSuccess
            .Should()
            .BeFalse();
    }

    [Test]
    public void CalcSizeForWords_WordWithUniqueCount_RightSizes()
    {
        var minFontSize = 10f;
        var maxFontSize = 30f;
        var wordsAndCount = new Dictionary<string, int>()
        {
            {
                "1234", 5
            },
            {
                "123rr", 3
            },
            {
                "qwe", 6
            },
            {
                "ddd", 8
            },
            {
                "zxcv", 15
            }
        };
        var step = (maxFontSize - minFontSize) / wordsAndCount.Count;

        var wordsAndSizesExpected = new Dictionary<string, float>()
        {
            {
                "1234", minFontSize + step * 1
            },
            {
                "123rr", minFontSize
            },
            {
                "qwe", minFontSize + step * 2
            },
            {
                "ddd", minFontSize + step * 3
            },
            {
                "zxcv", minFontSize + step * 4
            }
        };

        var wordsAndSizesActual = _wordsSizeCalculator.CalcSizeForWords(wordsAndCount, minFontSize, maxFontSize);

        
        using (new AssertionScope())
        {
            wordsAndSizesActual.IsSuccess.Should().BeTrue();
            wordsAndSizesActual.GetValueOrThrow().Should().BeEquivalentTo(wordsAndSizesExpected);
        }
    }

    [Test]
    public void CalcSizeForWords_OneWord_MinSizeForWord()
    {
        const float minFontSize = 10f;
        const float maxFontSize = 30f;
        var wordsAndCount = new Dictionary<string, int>()
        {
            {
                "1234", 5
            },
            {
                "qwe", 5
            },
            {
                "ddd", 5
            },
        };

        var wordsAndSizesExpected = new Dictionary<string, float>()
        {
            {
                "1234", minFontSize
            },
            {
                "qwe", minFontSize
            },
            {
                "ddd", minFontSize
            },
        };

        var wordsAndSizesActual = _wordsSizeCalculator.CalcSizeForWords(wordsAndCount, minFontSize, maxFontSize);

        using (new AssertionScope())
        {
            wordsAndSizesActual.IsSuccess.Should().BeTrue();
            wordsAndSizesActual.GetValueOrThrow().Should().BeEquivalentTo(wordsAndSizesExpected);
        }
    }


    [Test]
    public void CalcSizeForWords_TwoWordsWithEqualsCount_MinSizeForAllWord()
    {
        var minFontSize = 10f;
        var maxFontSize = 30f;
        var wordsAndCount = new Dictionary<string, int>()
        {
            {
                "1234", 3
            },
            {
                "123rr", 5
            },
            {
                "qwe", 5
            },
            {
                "ddd", 5
            },
            {
                "zxcv", 43
            },
            {
                "ssss", 15
            }
        };

        var step = (maxFontSize - minFontSize) / 4;
        var wordsAndSizesExpected = new Dictionary<string, float>()
        {
            {
                "1234", minFontSize
            },
            {
                "123rr", minFontSize + step * 1
            },
            {
                "qwe", minFontSize + step * 1
            },
            {
                "ddd", minFontSize + step * 1
            },
            {
                "zxcv", minFontSize + step * 3
            },
            {
                "ssss", minFontSize + step * 2
            }
        };

        var wordsAndSizesActual = _wordsSizeCalculator.CalcSizeForWords(wordsAndCount, minFontSize, maxFontSize);

        using (new AssertionScope())
        {
            wordsAndSizesActual.IsSuccess.Should().BeTrue();
            wordsAndSizesActual.GetValueOrThrow().Should().BeEquivalentTo(wordsAndSizesExpected);
        }
    }

    [Test]
    public void CalcSizeForWords_ManyWordsWithDifferentCountAndTreeWithEqualsCount_RightSizeForAllWord()
    {
        const float minFontSize = 10f;
        const float maxFontSize = 30f;
        var wordsAndCount = new Dictionary<string, int>()
        {
            {
                "1234", 5
            }
        };

        var wordsAndSizesExpected = new Dictionary<string, float>()
        {
            {
                "1234", minFontSize
            }
        };

        var wordsAndSizesActual = _wordsSizeCalculator.CalcSizeForWords(wordsAndCount, minFontSize, maxFontSize);

        using (new AssertionScope())
        {
            wordsAndSizesActual.IsSuccess.Should().BeTrue();
            wordsAndSizesActual.GetValueOrThrow().Should().BeEquivalentTo(wordsAndSizesExpected);
        }
    }
}