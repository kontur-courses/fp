using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultOfTask;
using TagsCloudPreprocessor;
using TagsCloudPreprocessor.Preprocessors;

namespace TagsCloudPreprocessorTests
{
    [TestFixture]
    public class SimplePreprocessor_Should
    {
        [Test]
        public void ExcludeForbiddenWords()
        {
            var excluder = A.Fake<IWordExcluder>();
            var excludedWordsHashSet = new HashSet<string> {"a", "b", "c", "d"};
            A.CallTo(() => excluder.GetExcludedWords()).Returns(excludedWordsHashSet.AsResult());

            var preprocessor = new WordsExcluder(excluder);
            var wordsToPreprocess = new List<string> {"a", "b", "e", "f"};

            var result = preprocessor
                .PreprocessWords(wordsToPreprocess)
                .GetValueOrThrow();
            var expectedResult = new List<string> {"e", "f"};

            result.ShouldBeEquivalentTo(expectedResult);
        }
    }
}