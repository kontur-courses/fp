using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer
{
    [TestFixture]
    public class ResultQueryExpression_Should
    {
        [Test]
        public void SupportLinqMethodChaining()
        {
            var res =
                "1358571172".ParseIntResult()
                    .SelectMany(i => Convert.ToString(i, 16).AsResult())
                    .SelectMany(hex => (hex + hex + hex + hex).ParseGuidResult());
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportLinqMethodChaining_WithResultSelector()
        {
            var res =
                "1358571172".ParseIntResult()
                    .SelectMany(i => Convert.ToString(i, 16).AsResult(), (i, hex) => new { i, hex })
                    .SelectMany(t => (t.hex + t.hex + t.hex + t.hex).ParseGuidResult());
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportQueryExpressions()
        {
            var res =
                from i in "1358571172".ParseIntResult()
                from hex in Convert.ToString(i, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            res.Should().BeOfType<Result<Guid>>();
            res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
        }

        [Test]
        public void SupportQueryExpressions_WithComplexSelect()
        {
            // ��� ��������, ���� �������� ������ ������ Query Expressions ��� Result<T>
            // �������� �������� �� ������������� i � select
            var res =
                from i in "1358571172".ParseIntResult()
                from hex in Convert.ToString(i, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select i + " -> " + guid;
            res.Should().BeOfType<Result<string>>();
            res.ShouldBeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenErrorAtTheEnd()
        {
            var res =
                from i in "0".ParseIntResult()
                from hex in Convert.ToString(i, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult("error is here")
                select guid;
            res.ShouldBeEquivalentTo(Result.Fail<Guid>("error is here"));
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenExceptionOnSomeStage()
        {
            var res =
                from i in "1358571172".ParseIntResult()
                from hex in Result.Of(() => Convert.ToString(i, 100500), "error is here")
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            res.ShouldBeEquivalentTo(Result.Fail<Guid>("error is here"));
        }

        [Test]
        public void ReturnFail_FromSelectMany_WhenErrorAtTheBeginning()
        {
            var res =
                from i in "UNPARSABLE".ParseIntResult("error is here")
                from hex in Convert.ToString(i, 16).AsResult()
                from guid in (hex + hex + hex + hex).ParseGuidResult()
                select guid;
            res.ShouldBeEquivalentTo(Result.Fail<Guid>("error is here"));
        }
    }
}