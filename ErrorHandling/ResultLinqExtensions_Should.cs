using System;
using FluentAssertions;
using NUnit.Framework;

namespace ErrorHandling
{
    [TestFixture]
    public class ResultLinqExtensions_Should
    {
        [Test]
        public void SupportLinqMethodChaining()
        {
            var res =
                Result.Of(() => int.Parse("1358571172"))
                    .SelectMany(i => Convert.ToString(i, 16))
                    .SelectMany(hex => Guid.Parse(hex + hex + hex + hex));
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqMethodChaining_WithResultOf()
        {
            var res =
                Result.Of(() => int.Parse("1358571172"))
                    .SelectMany(i => Convert.ToString(i, 16))
                    .SelectMany(hex => Result.Of(() => Guid.Parse(hex + hex + hex + hex)));
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqMethodChaining_WithResultSelector()
        {
            var res =
                Result.Of(() => int.Parse("1358571172"))
                    .SelectMany(i => Convert.ToString(i, 16), (i, hex) => new { i, hex })
                    .SelectMany(t => Guid.Parse(t.hex + t.hex + t.hex + t.hex));
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqMethodChaining_WithResultSelectorAndWithResultOf()
        {
            var res =
                Result.Of(() => int.Parse("1358571172"))
                    .SelectMany(i => Convert.ToString(i, 16))
                    .SelectMany(hex => Result.Of(() => Guid.Parse(hex + hex + hex + hex)), (hex, guid) => guid);
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqSyntax()
        {
            var res =
                from i in Result.Of(() => int.Parse("1358571172"))
                from hex in Convert.ToString(i, 16)
                from guid in Guid.Parse(hex + hex + hex + hex)
                select guid;
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqSyntax_ComplexScenario()
        {
            var res =
                from i in Result.Of(() => int.Parse("1358571172"))
                from hex in Convert.ToString(i, 16)
                from guid in Guid.Parse(hex + hex + hex + hex)
                select i + " -> " + guid;
            res.ShouldBeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenErrorAtTheEnd()
        {
            var res =
                from i in Result.Of(() => int.Parse("0"))
                from hex in Convert.ToString(i, 16)
                from guid in Result.Of(() => Guid.Parse(hex + hex + hex + hex), "guid format exception")
                select guid;
            res.ShouldBeEquivalentTo(Result.Fail<Guid>("guid format exception"));
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenExceptionOnSomeStage()
        {
            var res =
                from i in Result.Of(() => int.Parse("0"))
                from hex in Convert.ToString(i, 16)
                from guid in Guid.Parse(hex + hex + hex + hex) //FormatException!
                select guid;
            res.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenErrorAtTheBeginning()
        {
            var res =
                from i in Result.Of(() => int.Parse("UNPARSABLE"), "error is here")
                from hex in Convert.ToString(i, 16)
                from guid in Guid.Parse(hex + hex + hex + hex)
                select guid;
            res.ShouldBeEquivalentTo(Result.Fail<Guid>("error is here"));
        }
    }
}