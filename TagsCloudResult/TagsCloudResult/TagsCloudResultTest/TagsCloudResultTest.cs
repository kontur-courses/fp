using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ResultOf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TagsCloudContainer;

namespace TagsCloudResultTest
{
    public class Tests
    {
        [Test]
        public void CombineResults_ShouldNotThrowException_WhenKeyRepeats()
        {
            var dict = new Dictionary<string, object>();

            Action act = () => dict.CombineResults(Result.Ok(), "first")
            .Then(d => d.CombineResults(Result.Ok(), "first"));

            act.Should().NotThrow();
        }

        [Test]
        public void CombineResults_ShouldReturnResultWithErrorInfo_WhenKeyRepeats()
        {
            var dict = new Dictionary<string, object>();

            var res = dict.CombineResults(Result.Ok(), "first")
            .Then(d => d.CombineResults(Result.Ok(), "first"));

            res.IsSuccess.Should().BeFalse();
            res.Error.Should().Be("An item with the same key has already been added. Key: first");
        }

        [Test]
        public void GetImageSettings_ShouldReturnResultWithInfo_WhenErrorHappenedUpStack()
        {
            var settingErrorMessage = "какая-то ошибка ввода";
            var fakeClient = A.Fake<IClient>();
            A.CallTo(() => fakeClient.GetImageSize()).Returns(Result.Fail<Size>(settingErrorMessage));
            var clientController = new ClientControlFunc(fakeClient);

            var settings = clientController.GetImageSettings();

            settings.IsSuccess.Should().BeFalse();
            settings.Error.Should().Be("Данные некорректны. какая-то ошибка ввода");
        }

        [Test]
        public void ReadLineByLine_ShouldNotThrowException_WhenwhenFileIsNotExist()
        {
            var nonexistentFilePath = @"..\..\..\Files\t.txt";

            Action act = () => RecipientOfWords.ReadLineByLine(nonexistentFilePath);

            act.Should().NotThrow();
        }

        [Test]
        public void ReadLineByLine_ShouldReturnResultWithInfo_WhenwhenFileIsNotExist()
        {
            var nonexistentFilePath = @"..\..\..\Files\t.txt";

            var resText = RecipientOfWords.ReadLineByLine(nonexistentFilePath);

            Console.WriteLine(resText.Error);
            resText.IsSuccess.Should().BeFalse();
            resText.Error.Should().Be(@$"Could not find a part of the path '{Path.GetFullPath(nonexistentFilePath)}'.");
        }

        [Test]
        public void CreateCloud_ShouldReturnResultWithInfo_WhenCloudDidNotFitOnImage()
        {
            var words = new Dictionary<string, int>() { { "слово", 10 } };
            var settings = new ImageSettings(new Size(10, 1), FontFamily.GenericSansSerif, Color.Black, Color.White);

            var cloud = TagsCloud.CreateCloud(words, settings);

            cloud.IsSuccess.Should().BeFalse();
            cloud.Error.Should().Be("Облако тегов не влезло на изображение заданного размера");
        }
    }
}