using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.TextAnalyzer;
using YandexMystem.Wrapper;

namespace TagsCloudContainerTests
{
    internal class ToInitialFormNormalizerTests
    {
        private readonly Mysteam mysteam;

        public ToInitialFormNormalizerTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            mysteam = serviceProvider.GetRequiredService<Mysteam>();
        }

        [TestCase("слова", "слово")]
        [TestCase("слов", "слово")]
        [TestCase("слову", "слово")]
        [TestCase("словам", "слово")]
        public void ToInitialFormNormalizer_ShouldReturnInitialFormOfWord(string word, string initialForm)
        {
            new ToInitialFormNormalizer(mysteam).NormalizeWord(word).Should().BeEquivalentTo(initialForm);
        }
    }
}