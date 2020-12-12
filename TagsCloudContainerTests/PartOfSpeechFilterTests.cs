using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.App.TextAnalyzer;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainerTests
{
    internal class PartOfSpeechFilterTests
    {
        private readonly Mysteam mysteam;
        private readonly FilteringWordsSettings filteringSettings;

        public PartOfSpeechFilterTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            mysteam = serviceProvider.GetRequiredService<Mysteam>();
            filteringSettings = FilteringWordsSettings.Instance;
        }

        [TestCase(GramPartsEnum.Conjunction, "и")]
        [TestCase(GramPartsEnum.Interjection, "ах")]
        [TestCase(GramPartsEnum.NounPronoun, "он")]
        public void PartOfSpeechFilter_ShouldFilterPOS_IfPOSAreBoring(GramPartsEnum gramPart, string word)
        {
            filteringSettings.BoringGramParts = new[] {gramPart}.ToImmutableHashSet();
            new PartOfSpeechFilter(filteringSettings, mysteam).IsBoring(word).GetValueOrThrow().Should().BeTrue();
        }

        [TestCase(GramPartsEnum.Conjunction, "и")]
        [TestCase(GramPartsEnum.Interjection, "ах")]
        [TestCase(GramPartsEnum.NounPronoun, "он")]
        public void PartOfSpeechFilter_ShouldNotFilterPOS_IfPOSAreNotBoring(GramPartsEnum gramPart, string word)
        {
            filteringSettings.BoringGramParts = filteringSettings.BoringGramParts.Except(new[] {gramPart});
            new PartOfSpeechFilter(filteringSettings, mysteam).IsBoring(word).GetValueOrThrow().Should().BeFalse();
        }
    }
}