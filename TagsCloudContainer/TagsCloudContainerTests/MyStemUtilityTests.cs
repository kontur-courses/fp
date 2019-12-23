using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.PreprocessingWords;

namespace TagsCloudContainerTests
{
    public class MyStemUtilityTests
    {
        [Test]
        public void PreprocessingReturnLineFile()
        {
            var createProcess = A.Fake<ICreateProcess>();
            A.CallTo(() => createProcess.GetResult(null, null))
                .WithAnyArguments()
                .Returns(new[]
                {
                    "{\"analysis\":[{\"lex\":\"это\",\"gr\":\"SPRO,ед,сред,неод=(вин|им)\"}," +
                    "{\"lex\":\"это\",\"gr\":\"PART=\"},{\"lex\":\"этот\",\"gr\":\"APRO=(вин,ед,сред|им,ед,сред)\"}],\"text\":\"Это\"}",

                    "{\"analysis\":[{\"lex\":\"строка\",\"gr\":\"S,жен,неод=им,ед\"}],\"text\":\"строки\"}"
                });

            var stemUtility = new MyStemUtility(createProcess);
            stemUtility.Preprocessing(new[] {"это", "строки"}).GetValueOrThrow().ToArray().Should()
                .BeEquivalentTo("строка");
        }
        
        [Test]
        public void PreprocessingReturnResultError_WhenCreateProcessReturnNull()
        {
            var createProcess = A.Fake<ICreateProcess>();
            A.CallTo(() => createProcess.GetResult(null, null))
                .WithAnyArguments()
                .Returns(null);

            var stemUtility = new MyStemUtility(createProcess);
            stemUtility.Preprocessing(new[] {"строка"}).IsSuccess.Should().BeFalse();
        }
        
        [Test]
        public void PreprocessingReturnResultError_WhenCreateProcessThrowsException()
        {
            var createProcess = A.Fake<ICreateProcess>();
            A.CallTo(() => createProcess.GetResult(null, null))
                .WithAnyArguments()
                .Throws<Exception>();
            
            var stemUtility = new MyStemUtility(createProcess);
            stemUtility.Preprocessing(new[] {"строка"}).IsSuccess.Should().BeFalse();
        }
    }
}