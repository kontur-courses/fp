using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud;
using TagsCloud.Validators;

namespace TagsCloudTests.ValidatorsShould
{
    [TestFixture]
    public class ClientValidatorShould
    {
        private ClientValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new ClientValidator();
        }

        [Test]
        public void ValidatorCreate_ShouldHasOnlyRussianSymbolsInCharSet()
        {
            var pattern = @"[А-я]+";
            var charSet = validator.ValidChars;

            var line = new string(charSet);
            var res = Regex.Match(line, pattern);

            res.Groups.Count.Should().Be(1);
            res.Groups[0].Value.Length.Should().Be(64);
            // 32 буквы в верхнем регистре и 32 в нижнем
        }

        [TestCase("ПримUер")]
        [TestCase("Прим ер")]
        [TestCase("!Пример!")]
        [TestCase("'/.")]
        [TestCase("Example")]
        public void ValidateRussianInput_WrongLine_ShouldThrowException(string line)
        {
            var res = validator.ValidateRussianInput(line);

            ErrorValidate(res, "Invalid word");
        }

        [Test]
        public void ValidateRussianInput_CommonInput_CorrectResult()
        {
            var res = validator.ValidateRussianInput("Пример");

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be("Пример");
        }

        [TestCase(@"File?")]
        [TestCase(@"Фа:йл")]
        [TestCase(@"*Файл")]
        [TestCase(@"Фа|йл")]
        [TestCase(@"Ф>айл")]
        public void ValidateWrongSymbolsInPath_LineWithWrongSymbols_ShouldThrowException(string line)
        {
            var res = validator.ValidateWrongSymbolsInFileName(line);

            ErrorValidate(res, "Wrong symbols in path name");
        }

        [TestCase("Файл с пробелами")]
        [TestCase("English")]
        [TestCase("КАПСОМ")]
        [TestCase("Странные.символы_в!Названии")]
        public void ValidateWrongSymbolsInPath_CommonInput_CorrectResult(string line)
        {
            var res = validator.ValidateWrongSymbolsInFileName(line);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(line);
        }

        [Test]
        public void ValidateRightTextExtension_ExtensionWithoutDot_ShouldThrowException()
        {
            var res = validator.ValidateRightTextExtension("txt");

            ErrorValidate(res, @"A dot in the extension name is not found. 
Wrong extension. 
Expected only .txt files");
        }

        [Test]
        public void ValidateRightTextExtension_UnknownExtension_ShouldThrowException()
        {
            var res = validator.ValidateRightTextExtension(".docs");

            ErrorValidate(res, "Wrong extension. Expected only .txt files");
        }

        [Test]
        public void ValidateRightTextExtension_CommonInput_CorrectResult()
        {
            var res = validator.ValidateRightTextExtension(".txt");

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(".txt");
        }

        [Test]
        public void ValidateRightPictureExtension_ExtensionWithoutDot_ShouldThrowException()
        {
            var res = validator.ValidateRightPictureExtension("png");

            ErrorValidate(res, @"A dot in the extension name is not found. 
Wrong extension. 
Expected only .png .jpg files");
        }

        [Test]
        public void ValidateRightPictureExtension_UnknownExtension_ShouldThrowException()
        {
            var res = validator.ValidateRightPictureExtension(".bmp");

            ErrorValidate(res, "Wrong extension. Expected only .png .jpg files");
        }

        [TestCase(".png")]
        [TestCase(".jpg")]
        public void ValidateRightPictureExtension_CommonInput_CorrectResult(string line)
        {
            var res = validator.ValidateRightPictureExtension(line);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(line);
        }

        public static void ErrorValidate<T>(ResultHandler<T> handler, string errorMessage)
        {
            handler.Invoking(x => x.Value)
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(errorMessage);
        }
    }
}