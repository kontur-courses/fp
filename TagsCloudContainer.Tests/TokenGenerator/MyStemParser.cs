using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TokensGenerator;

namespace TagsCloudContainer.Tests.TokenGenerator
{
    [TestFixture]
    public class MyStemParser_Should
    {
        private MyStemParser tokenParser;
        private string word;
        private IMysteam mysteam;

        [SetUp]
        public void SetUp()
        {
            word = "слово";
            tokenParser =  new MyStemParser(new Mysteam());
        }

        [Test]
        public void GetTokens_WhenNull_ThrowArgumentException()
        {
            Action act = () => { tokenParser.GetTokens(null); };
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetTokens_WhenEmpty()
        {
            tokenParser.GetTokens("").GetValueOrThrow().Should().HaveCount(0);
        }

        [Test]
        public void GetTokens_WhenWord_ReturnWord()
        {
            tokenParser.GetTokens(word).GetValueOrThrow().First().Should().Be(word);
        }

        [Test]
        public void GetTokens_WhenOneWord_ContainOneTokenWithCountIsOne()
        {
            var token = tokenParser.GetTokens(word);
            token.GetValueOrThrow().Should().HaveCount(1);
        }

        [Test]
        public void GetTokens_WhenDuplicate_ContainOneToken()
        {
            var result = tokenParser.GetTokens(word + Environment.NewLine + word);
            result.GetValueOrThrow().Should().HaveCount(2);
        }

        [Test]
        public void GetTokens_WhenDuplicate_TokenCountIsTwo()
        {
            var token = tokenParser.GetTokens(word + Environment.NewLine + word);
            token.GetValueOrThrow().Should().HaveCount(2);
        }

        [Test]
        public void GetTokens_WhenPunctuation_ShouldntContainPunctuation()
        {
            var text = @"слова, с - пунктуацией точка.";
            var token = tokenParser.GetTokens(text);
            token.GetValueOrThrow().Should().NotContain(new []{",",".", "-"});
        }

        [Test]
        public void GetTokens_WhenWordWithCapitalLetter_ShouldContainWordWithLowerSymbol()
        {
            var text = "Слова С Большой Буквы";
            var token = tokenParser.GetTokens(text);
            token.GetValueOrThrow().All(el => el.ToLower() == el).Should().BeTrue();
        }
    }
}