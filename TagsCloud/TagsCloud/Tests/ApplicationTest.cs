using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.ErrorHandler;
using TagsCloud.Visualization.Tag;

namespace TagsCloud.Tests
{
    [TestFixture]
    public class ApplicationTest
    {
        private string _input;

        private readonly string _text =
            "лимон\nЛиМоНа\nЛИМОН\nчаЙ\nчая\nМёд\nс\nимбирь\nимбирь\nимбирём\nбегал\nбегает\nбегают\nбег";

        [OneTimeSetUp]
        public void FirstSetUp()
        {
            var rnd = new Random();
            _input = $"test{rnd.Next(0, 2000000)}.txt";
            while (File.Exists(_input)) _input = $"test{rnd.Next(0, 2000000)}.txt";
            using (var sw = new StreamWriter(_input, false, Encoding.Default))
            {
                sw.Write(_text);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            File.Delete(_input);
        }

        private static Result<IEnumerable<Tag>> GetTags(IEnumerable<string> args)
        {
            var app = GetApplication(args);
            return app.Value.GetTags();
        }
        private static Result<Application> GetApplication(IEnumerable<string> args)
        {
            return Result.Ok(args)
                .Then(Options.Parse)
                .Then(ContainerConstructor.Configure)
                .Then(c => c.Resolve<Application>());
        }

        [Test]
        public void GetTags_ReturnsCorrectTagsCount_OnDefaultOptions()
        {
            var tags = GetTags(new[] {"--file", _input});
            tags.Value.ToList().Count.Should().Be(11);
        }

        [Test]
        public void GetTags_ReturnsCorrectTagsCount_OnInfinitiveFormOption()
        {
            var tags = GetTags(new[] {"--file", _input, "--inf"});
            tags.Value.ToList().Count.Should().Be(6);
        }


        [Test]
        public void GetTags_ReturnsTagsWithLoweredWords()
        {
            var tags = GetTags(new[] {"--file", _input});
            foreach (var tag in tags.Value) (tag.Word.ToLower() == tag.Word).Should().BeTrue();
        }

        [Test]
        public void GetTags_ReturnTagsWithCorrectTagsSize()
        {
            var tags = GetTags(new[] {"--file", _input});
            foreach (var tag1 in tags.Value)
            foreach (var tag2 in tags.Value.Where(tag2 => tag1.Frequency > tag2.Frequency))
                tag1.Size.Should().BeGreaterThan(tag2.Size);
        }

        [Test]
        public void GetTags_ThrowException_OnWrongArgs()
        {
            var result = GetApplication(new[] {"--pew"}).Error;
            result.Should().Be("Wrong options");
        }
        
        [Test]
        public void GetTags_ThrowExceptio_OnWrongArgs()
        {
            var result = GetApplication(new[] {"-f unkn.txt"}).Error;
            result.Should().Be("File not found");
        }
    }
}