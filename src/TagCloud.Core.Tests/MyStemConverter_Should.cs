﻿using System.IO;
using FluentAssertions;
using FunctionalStuff.TestingExtensions;
using MyStem.Wrapper.Workers.Lemmas;
using MyStem.Wrapper.Wrapper;
using NUnit.Framework;
using TagCloud.Core.Text.Preprocessing;

namespace TagCloud.Core.Tests
{
    // ReSharper disable once InconsistentNaming
    public class MyStemConverter_Should
    {
        private string executablePath;
        private MyStemWordsConverter converter;

        [SetUp]
        public void SetUp()
        {
            executablePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "../../../../dlls/", "mystem.exe");
            converter = new MyStemWordsConverter(
                new Lemmatizer(new MyStemBuilder(executablePath)));
        }

        [Test]
        public void SingleWord_Normalize()
        {
            converter.Normalize(new[] {"упячкой"})
                .ShouldBeSuccessful()
                .Which
                .Value()
                .Should()
                .BeEquivalentTo("упячка");
        }

        [Test]
        public void SeveralWords_NormalizeEach()
        {
            converter.Normalize(new[] {"упячкой", "бошечки", "стирателей"})
                .ShouldBeSuccessful()
                .Which
                .Value()
                .Should()
                .BeEquivalentTo("упячка", "бошечка", "стиратель");
        }

        [Test]
        public void AlreadyNormalized_DontModify()
        {
            converter.Normalize(new[] {"упячка"})
                .ShouldBeSuccessful()
                .Which
                .Value()
                .Should()
                .BeEquivalentTo("упячка");
        }

        [Test]
        public void EnglishWords_DontModify()
        {
            converter.Normalize(new[] {"matches"})
                .ShouldBeSuccessful()
                .Which
                .Value()
                .Should()
                .BeEquivalentTo("matches");
        }
    }
}