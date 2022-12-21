using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Validators;

namespace TagsCloudTests.ValidatorsShould
{
    [TestFixture]
    public class FileValidatorShould
    {
        private FileValidator validator = new FileValidator();
        private readonly string basePath = @"..\..\..\..\";

        [Test]
        public void VerifyFileExistence_NullPath()
        {
            var res = validator.VerifyFileExistence(null);

            ClientValidatorShould.ErrorValidate(res, "Null path");
        }

        [Test]
        public void VerifyFileExistence_FileIsNotExist()
        {
            var fileName = GetNewRandomFileName();

            File.Delete(basePath + fileName);
            var res = validator.VerifyFileExistence(basePath + fileName);

            ClientValidatorShould.ErrorValidate(res, "File was not found");
        }

        [Test]
        public void VerifyFileExistence_CommonInput_CorrectResult()
        {
            var fileName = GetNewRandomFileName();

            using (File.Create(basePath + fileName))
            {
            }

            var res = validator.VerifyFileExistence(basePath + fileName);
            File.Delete(basePath + fileName);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().Be(basePath + fileName);
        }

        private string GetNewRandomFileName()
        {
            return "VerifyFileExistence TestFile №" + new Random().Next(0, 1000) + ".txt";
        }
    }
}