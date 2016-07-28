using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace FP.MaybeImpl
{
	[TestFixture]
	public class Result_Should
	{
		[Test]
		public void Create_Ok()
		{
			var r = Result.Ok(42);
			r.IsSuccess.Should().BeTrue();
			r.Value.Should().Be(42);
		}

		[Test]
		public void Create_Fail()
		{
			var r = Result.Fail<int>("123");

			r.IsSuccess.Should().BeFalse();
			r.Error.Should().Be("123");
		}

		[Test]
		public void RunOnSuccess_WhenOk()
		{
			var res = Result.Ok(42)
				.OnSuccess(n => n + 10);
			res.ShouldBeEquivalentTo(Result.Ok(52));
		}

		[Test]
		public void SkipOnSuccess_WhenFail()
		{
			var fail = Result.Fail<int>("ошибка");
			var res = fail.OnSuccess(n =>
			{
				Assert.Fail("should not be executed");
				return n;
			});
			res.ShouldBeEquivalentTo(fail);
		}

		[Test]
		public void RunOnFail_WhenFail()
		{
			var fail = Result.Fail<int>("ошибка");
			var errorHandler = A.Fake<Action<Result<int>>>();

			var res = fail.OnFail(errorHandler);

			A.CallTo(() => errorHandler(null)).WithAnyArguments().MustHaveHappened();
			res.ShouldBeEquivalentTo(fail);
		}

		[Test]
		public void SkipOnFail_WhenOk()
		{
			var ok = Result.Ok(42);

			var res = ok.OnFail(v => { Assert.Fail("Should not be called"); });

			res.ShouldBeEquivalentTo(ok);
		}

		[Test]
		public void ReturnsFail_FromResultOf_OnException()
		{
			var res = Result.Of<int>(() => { throw new Exception("123"); });

			res.ShouldBeEquivalentTo(Result.Fail<int>("123"));
		}

		[Test]
		public void ReturnsFailWithCustomMessage_FromResultOf_OnException()
		{
			var res = Result.Of<int>(() => { throw new Exception("123"); }, "42");

			res.ShouldBeEquivalentTo(Result.Fail<int>("42"));
		}

		[Test]
		public void ReturnsOk_FromResultOf_WhenNoException()
		{
			var res = Result.Of(() => 42);

			res.ShouldBeEquivalentTo(Result.Ok(42));
		}

		[Test]
		public void RunOnSuccess_WhenOk_Scenario()
		{
			var res =
				Result.Of(() => int.Parse("1358571172"))
					.OnSuccess(i => Convert.ToString(i, 16))
					.OnSuccess(hex => Guid.Parse(hex + hex + hex + hex));
			res.ShouldBeEquivalentTo(Result.Ok(Guid.Parse("50FA26A450FA26A450FA26A450FA26A4")));
		}

		[Test]
		public void RunOnSuccess_WhenOk_ComplexScenario()
		{
			var parsed = Result.Of(() => int.Parse("1358571172"));
			var res = parsed
				.OnSuccess(i => Convert.ToString(i, 16))
				.OnSuccess(hex => parsed.Value + " -> " + Guid.Parse(hex + hex + hex + hex));
			res.ShouldBeEquivalentTo(Result.Ok("1358571172 -> 50fa26a4-50fa-26a4-50fa-26a450fa26a4"));
		}
		/*
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
		*/
	}
}