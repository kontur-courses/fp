using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagCloudApp;
using TagCloudCreation;

namespace TagCloudTests
{
    [TestFixture]
    public class SelectedBoringWordsRemover_Should
    {
        [SetUp]
        public void SetUp()
        {
            var fake = A.Fake<ITextReader>();
            A.CallTo(() => fake.ReadWords(A<string>.Ignored))
             .Returns(new List<string> {"a"});
            remover = new SelectedBoringWordsRemover(fake);
        }

        private SelectedBoringWordsRemover remover;

        [Test]
        public void ReturnNull_IfWordIsBoring()
        {
            remover.PrepareWord("a", new TagCloudCreationOptions(null, "path"))
                   .Should()
                   .BeNull();
        }

        [Test]
        public void ReturnWord_IfWordIsNotBoring()
        {
            remover.PrepareWord("b", new TagCloudCreationOptions(null, "path"))
                   .Should()
                   .Be("b");
        }
    }
}
