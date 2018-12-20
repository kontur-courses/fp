using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.Processing.Converting;

namespace TagsCloudContainerTests.Processing
{
    public class WordConverterShould
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (dir != null)
            {
                Environment.CurrentDirectory = dir;
                Directory.SetCurrentDirectory(dir);
            }
            else
                throw new NullReferenceException(
                    "Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) returns null");
        }

        [Test]
        public void ConvertToInitialForm()
        {
            var words = new[] {"подруге", "бутылки", "заводом", "оттепель", "брате"};
            var expected = new[] {"подруга", "бутылка", "завод", "оттепель", "брат"};

            new InitialFormConverter().Convert(words).Should().BeEquivalentTo(expected);
        }
    }
}