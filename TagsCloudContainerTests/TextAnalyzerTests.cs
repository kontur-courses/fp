using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.Infrastructure.TextAnalyzer;

namespace TagsCloudContainerTests
{
    internal class TextAnalyzerTests
    {
        private readonly ITextAnalyzer textAnalyzer;

        public TextAnalyzerTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            textAnalyzer = serviceProvider.GetRequiredService<ITextAnalyzer>();
        }

        [Test]
        public void TextAnalyzer_ShouldReturnCorrectFrequencyDictionary()
        {
            var text = new[] {"слова", "слово", "количество"};
            textAnalyzer
                .GenerateFrequencyDictionary(text)
                .GetValueOrThrow()
                .Should()
                .HaveCount(2)
                .And.ContainKeys("слово", "количество").And.ContainValues(2.0 / 3, 1.0 / 3);
        }
    }
}