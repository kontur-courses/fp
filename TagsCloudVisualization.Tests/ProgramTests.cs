using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Common.ErrorHandling;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        private const string TestBigTextPath = @"txt\Text_Большой_текст.txt";
        private List<string> args;

        [SetUp]
        public void SetUp()
        {
            var dirTestData = TestContext.CurrentContext.TestDirectory + @"\TestData\";
            var inputFile = dirTestData + TestBigTextPath;
            var outputFile = dirTestData + "test.png";

            args = new List<string>
            {
                "create-cloud",
                "-i", inputFile,
                "-o", outputFile
            };
        }

        [Test]
        public void Program_CheckExecutionTime_ForCreateCloud()
        {
            var actual = CheckExecutionTime(args.ToArray())
                .OnSuccess(value => Console.WriteLine($"Время выполнения create-cloud - {value.Elapsed}."))
                .OnFail(Console.WriteLine);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Program_CheckExecutionTime_ForShowDemo()
        {
            var actual = CheckExecutionTime(new[]
                    {"show-demo", "-o", TestContext.CurrentContext.TestDirectory + @"\TestData\"})
                .OnSuccess(value => Console.WriteLine($"Время выполнения show-demo - {value.Elapsed}."))
                .OnFail(Console.WriteLine);

            actual.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void Program_ShouldThrowException_WhenWrongInput()
        {
            args[2] += "1";
            var actual = CheckExecutionTime(args.ToArray())
                .OnFail(Console.WriteLine);

            actual.IsSuccess.Should().BeFalse();
            actual.Should().BeEquivalentTo(Result.Fail<int>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<int>)));
        }

        [Test]
        public void Program_ShouldThrowException_WhenWrongOutput()
        {
            args[4] += ":";
            var actual = CheckExecutionTime(args.ToArray())
                .OnFail(Console.WriteLine);

            actual.IsSuccess.Should().BeFalse();
            actual.Should().BeEquivalentTo(Result.Fail<int>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<int>)));
        }

        [TestCase("-w", "0", TestName = "Zero width")]
        [TestCase("-w", "-100", TestName = "Negative width")]
        [TestCase("-h", "0", TestName = "Zero height")]
        [TestCase("-h", "-100", TestName = "Negative height")]
        [TestCase("--bgColor", "Hoccasin", TestName = "Wrong background color")]
        [TestCase("--fgColors", "Indigo;Hoccasin;BlueViolet", TestName = "Wrong foreground color")]
        [TestCase("--fonts", "Calibri;Ambria;Comic Sans MS", TestName = "Wrong font name")]
        [TestCase("--size", "0", TestName = "Zero font size")]
        [TestCase("--size", "-10", TestName = "Negative font size")]
        [TestCase("--scatter", "-10", TestName = "Negative font size scatter")]
        public void Program_ShouldThrowException_WhenWrongOptionalArgument(string parameter, string value)
        {
            args.AddRange(new[] {parameter, value});
            var actual = RunProgram(args.ToArray())
                .OnFail(Console.WriteLine);

            actual.IsSuccess.Should().BeFalse();
            actual.Should().BeEquivalentTo(Result.Fail<int>(string.Empty),
                options => options
                    .Excluding(ctx => ctx.Error)
                    .ComparingByMembers(typeof(Result<int>)));
        }

        private static Result<Stopwatch> CheckExecutionTime(string[] args)
        {
            var timer = new Stopwatch();
            return Result.OfAction(() => timer.Start())
                .Then(_ => RunProgram(args))
                .Then(_ => timer.Stop())
                .Then(_ => timer);
        }

        private static Result<int> RunProgram(string[] args)
        {
            var returnCode = Program.Main(args);
            return returnCode == 0
                ? Result.Ok(1)
                : Result.Fail<int>($"Программа завершилась с кодом {returnCode}");
        }
    }
}