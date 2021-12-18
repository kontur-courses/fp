using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using NUnit.Framework;
using ResultMonad;
using TagsCloudVisualization.Module;
using TagsCloudVisualization.WordsProvider;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        private const string ImageFile = "TagsCloud";
        private static readonly string WordsFile = Path.Combine(Directory.GetCurrentDirectory(), "Words.txt");
        private readonly TagsCloudVisualisationSettings _settings = new()
        {
            WordsProvider = new WordsFromTxtFileProvider(WordsFile),
            BoringWords = new[] { "a", "b", "c" }
        };

        [OneTimeSetUp]
        public void BeforeAll()
        {
            File.Create(WordsFile).Close();
        }

        [OneTimeTearDown]
        public void AfterAll()
        {
            File.Delete(WordsFile);
        }

        [Test]
        public void TagsCloudVisualisation_ShouldCreateImageWithTagsCloud()
        {
            var uniqueWords = new[]
                {
                    "A",
                    "B",
                    "C"
                }.Concat(Enumerable.Range(0, 100).Select(i => $"Word{i}"))
                .ToArray();
            var generated = GenerateWordsList(uniqueWords, 1000);
            File.WriteAllLines(WordsFile, generated);

            new ContainerBuilder()
                .RegisterTagsClouds(_settings)
                .RegisterImageCreation(ImageFile)
                .Then(b => b.Build().Resolve<TagsCloudVisualizer>())
                .Then(v => v.Visualize(20))
                .OnFail(Assert.Fail);
        }

        private static IEnumerable<string> GenerateWordsList(IList<string> uniqueWords, int count)
        {
            var rnd = new Random();
            return Enumerable.Range(0, count).Select(_ => uniqueWords[rnd.Next(uniqueWords.Count)]);
        }
    }
}