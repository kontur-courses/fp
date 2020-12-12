using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.App.TextAnalyzer;
using TagsCloudContainer.Infrastructure.TextAnalyzer;
using YandexMystem.Wrapper;

namespace TagsCloudContainerTests
{
    internal class EnumerableExtensionsTests
    {
        private readonly Mysteam mysteam;
        private readonly FilteringWordsSettings filteringSettings;

        public EnumerableExtensionsTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            mysteam = serviceProvider.GetRequiredService<Mysteam>();
            filteringSettings = FilteringWordsSettings.Instance;
        }

        [Test]
        public void FilterOutBoringWords_ShouldFilterSuccessfully_IfAllWordsAreValid()
        {
            new[] {"слова", "и", "да", "по"}
                .FilterOutBoringWords(new[] {new PartOfSpeechFilter(filteringSettings, mysteam)})
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo("слова");
        }

        [Test]
        public void FilterOutBoringWords_ShouldReturnResultWithError_IfSomeWordIsInvalid()
        {
            var invalidWord = "jhghg";
            new[] {"слова", invalidWord}
                .FilterOutBoringWords(new[] {new PartOfSpeechFilter(filteringSettings, mysteam)})
                .Error
                .Should()
                .BeEquivalentTo($"Can't identify part of speech of word: {invalidWord}");
        }

        [Test]
        public void NormalizeWords_ShouldNormalizeSuccessfully_IfAllWordsAreValid()
        {
            new[] { "слова", "формы" }
                .NormalizeWords(new[] { new ToInitialFormNormalizer(mysteam) })
                .GetValueOrThrow()
                .Should()
                .BeEquivalentTo("слово", "форма");
        }

        [Test]
        public void NormalizeWords_ShouldReturnResultWithError_IfSomeWordIsInvalid()
        {
            var invalidWord = "jhghg";
            new[] { "слова", invalidWord }
                .NormalizeWords(new IWordNormalizer[] { new ToLowerWordNormalizer(), 
                    new ToInitialFormNormalizer(mysteam) })
                .Error
                .Should()
                .BeEquivalentTo($"Can't normalize word {invalidWord} to initial form");
        }
    }
}
