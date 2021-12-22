using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Common.FileReaders;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class TextFileReaderTests
    {
        private TextFileReader reader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reader = new TextFileReader();
        }

        [Test]
        public void ReadFile_ShouldReadTextFileCorrectly()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\txt\Test_Облако.txt";
            const string expected = "Облако\r\nВ облаке\r\nНа облаке\r\nЗа обЛакОм\r\nОб облакЕ\r\n" +
                                    "Я облако\r\nМы облака\r\nНикаких облаков\r\nКаким-нибудь облаком\r\nНеким облаком";

            var actual = reader.ReadFile(filePath);

            actual.Should().BeEquivalentTo(Result.Ok(expected));
        }

        [Test]
        public void ReadFile_ShouldThrowException_WhenFileNotFound()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\txt\NoFile.txt";

            var actual = reader.ReadFile(filePath);

            Console.WriteLine(actual.Error);
            actual.Should().BeEquivalentTo(Result.Fail<string>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<string>)));
        }

        [Test]
        public void ReadLines_ShouldReadTextFileCorrectly()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\txt\Test_Облако.txt";
            var expected = ("Облако\r\nВ облаке\r\nНа облаке\r\nЗа обЛакОм\r\nОб облакЕ\r\n" +
                            "Я облако\r\nМы облака\r\nНикаких облаков\r\nКаким-нибудь облаком\r\nНеким облаком")
                .Split()
                .AsEnumerable();

            var actual = reader.ReadLines(filePath);

            actual.Should().BeEquivalentTo(Result.Ok(expected),
                options => options.ComparingByMembers(typeof(Result<IEnumerable<string>>)));
        }

        [Test]
        public void ReadLines_ShouldThrowException_WhenFileNotFound()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\txt\NoFile.txt";

            var actual = reader.ReadLines(filePath);

            Console.WriteLine(actual.Error);
            actual.Should().BeEquivalentTo(Result.Fail<IEnumerable<string>>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<IEnumerable<string>>)));
        }
    }
}