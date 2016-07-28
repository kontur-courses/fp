using System;

namespace FP.MaybeImpl
{
	public static class Result{
		public static Result<T> Ok<T>(T value)
		{
			return new Result<T>(null, value);
		}

		public static Result<T> Fail<T>(string e)
		{
			return new Result<T>(e);
		}

		public static Result<T> Of<T>(Func<T> f, string error)
		{
			try
			{
				return Ok(f());
			}
			catch (Exception)
			{
				return Fail<T>(error);
			}
		}

		public static Result<T> Of<T>(Func<T> f)
		{
			try
			{
				return Ok(f());
			}
			catch (Exception e)
			{
				return Fail<T>(e.Message);
			}
		}

		public static Result<TRes> OnSuccess<T, TRes>(this Result<T> input, Func<T, TRes> map)
		{
			throw new NotImplementedException("TODO");
		}

		public static Result<TRes> OnSuccess<T, TRes>(this Result<T> input, Func<T, Result<TRes>> map)
		{
			throw new NotImplementedException("TODO");
		}

		public static Result<T> OnFail<T>(this Result<T> input, Action<Result<T>> handleError)
		{
			throw new NotImplementedException("TODO");
		}

		//TODO SelectMany extension method
	}

	public class Result<T>
	{
		public Result(string error, T value = default(T))
		{
			Error = error;
			Value = value;
		}

		public string Error { get; }
		public T Value { get; }
		public bool IsSuccess => Error == null;
	}
}