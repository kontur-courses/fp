using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework.Internal;
using TagCloudGenerator.Clients;
using TagCloudGenerator.GeneratorCore;
using TagCloudGenerator.GeneratorCore.CloudVocabularyPreprocessors;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.GeneratorCore.Tags;
using TagCloudGenerator.ResultPattern;
using TagCloudGenerator_Tests.TestFixtures;
using TagType = TagCloudGenerator_Tests.WrongVisualization.TagType;

namespace TagCloudGenerator_Tests
{
    public static class TestsHelper
    {
        public static Color BackgroundColor => Color.White;

        public static Dictionary<TagType, TagStyle> TagStyleByTagType => new Dictionary<TagType, TagStyle>
        {
            [TagType.Normal] = new TagStyle(Color.LightGray, null),
            [TagType.SecondWrong] = new TagStyle(Color.Crimson, null),
            [TagType.FirstWrong] = new TagStyle(Color.Teal, null)
        };

        public static ITagCloudOptions<ITagCloud> DefaultCloudOptions =>
            new CloudOptions<CommonWordsCloud>
            {
                CloudVocabularyFilename = "CommonWordsCloudFilename.txt",
                ImageSize = "800x600",
                ExcludedWordsVocabularyFilename = "ExcludedWords.txt",
                ImageFilename = "CommonWordsTagCloud.png",
                GroupsCount = 2,
                MutualFont = "Bahnschrift SemiLight",
                BackgroundColor = "#FFFFFFFF",
                FontSizes = "30_18",
                TagColors = "#FF000000_#FF000000"
            };

        public static (Rectangle, Rectangle)? GetAnyPairOfIntersectingRectangles(Rectangle[] rectangles)
        {
            for (var i = 0; i < rectangles.Length; i++)
                for (var j = i + 1; j < rectangles.Length; j++)
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return (rectangles[i], rectangles[j]);
            return null;
        }

        public static void HandleErrors(Action<string> errorHandler, params IResult[] results)
        {
            foreach (var result in results)
                if (!result.IsSuccess)
                    errorHandler(result.Error);
        }

        public static IEnumerable<T> SelectValues<T>(IEnumerable<Result<T>> results)
        {
            foreach (var result in results)
            {
                result.Error.Should().BeNull();
                yield return result.Value;
            }
        }

        public static string GetRandomString(int length, Randomizer randomizer)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                  .Select(charsString => charsString[randomizer.Next(charsString.Length)])
                                  .ToArray());
        }

        public static CloudVocabularyPreprocessor PreprocessorConstructor(TagCloudContext cloudContext)
        {
            CloudVocabularyPreprocessor preprocessor = new ExcludingPreprocessor(null, cloudContext);
            preprocessor = new ToLowerPreprocessor(preprocessor);

            return preprocessor;
        }
    }
}