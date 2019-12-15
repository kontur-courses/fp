using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagCloud;
using TagCloud.TextConversion;
using TagCloud.TextFilterConditions;
using TagCloud.TextFiltration;
using TagCloud.TextParser;
using TagCloud.TextProvider;

namespace TagCloud_Should
{
    [TestFixture]
    public class TextFilter_Should
    {
        private TextFilter textFilter;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ToLowerCaseConversion>().As<ITextConversion>();
            builder.RegisterType<WordLengthCondition>().As<IFilterCondition>().SingleInstance();
            builder.RegisterType<TextFilter>().AsSelf().SingleInstance();
            builder.RegisterType<TextConverter>().AsSelf().SingleInstance();
            builder.RegisterType<TextParser>().As<ITextParser>().SingleInstance();
            builder.RegisterType<UnitTestsTextProvider>().As<ITextProvider>().SingleInstance();
            builder.RegisterType<FrequencyDictionaryMaker>().AsSelf().SingleInstance();
            builder.RegisterType<BlacklistMaker>().AsSelf().SingleInstance()
                .WithProperty("BlackList", new HashSet<string> {"blacklistWord"});
            builder.RegisterType<BlacklistSettings>().AsSelf().SingleInstance()
                .WithProperty("FilesWithBannedWords", new HashSet<string>());

            var container = builder.Build();
            textFilter = container.Resolve<TextFilter>();
        }

        [Test]
        public void ShouldRemoveBannedWords()
        {
            textFilter.FilterWords().Contains("blacklistWord").Should().BeFalse();
        }

        [Test]
        public void ShouldRemoveSmallWords()
        {
            textFilter.FilterWords().Contains("b").Should().BeFalse();
        }

        [Test]
        public void ShouldNotRemoveNotBannedWords()
        {
            textFilter.FilterWords().Should().Contain("word3");
        }
    }
}