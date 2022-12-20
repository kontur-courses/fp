using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.Validators;

namespace TagsCloudTests.ValidatorsShould
{
    [TestFixture]
    public class MorphsValidatorShould
    {
        private MorphsValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new MorphsValidator();
        }

        [Test]
        public void ParseOnMorphs_NullEnum_ShouldThrowException()
        {
            var words = new List<string>();

            var res = validator.ParseOnMorphs(words);

            ClientValidatorShould.ErrorValidate(res, "Empty words enum");
        }

        [Test]
        public void ParseOnMorphs_CommonInput_ShouldSuccess()
        {
            var words = new[] { "Один", "Два", "Три" };

            var res = validator.ParseOnMorphs(words);

            res.IsSuccess.Should().BeTrue();
            res.Value.Should().NotBeNull();
        }
    }
}