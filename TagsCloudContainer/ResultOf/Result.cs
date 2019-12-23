using System;

namespace ResultOf
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(string error, T value = default)
        {
            Error = error;
            Value = value;
        }

        public static implicit operator Result<T>(T v)
        {
            return Result.Ok(v);
        }

        public string Error { get; }
        internal T Value { get; }

        public T GetValueOrThrow()
        {
            if (IsSuccess) return Value;
            throw new InvalidOperationException($"No value. Only Error {Error}");
        }

        public bool IsSuccess => Error == null;
    }

    public static class Result
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return Ok(value);
        }

        public static Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Ok(obj)
                : Fail<T>(errorMessage);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }

        public static Result<None> Ok()
        {
            return Ok<None>(null);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
        }

        public static Result<T> Of<T>(Func<T> f, string error = null)
        {
            try
            {
                return Ok(f());
            }
            catch (Exception e)
            {
                return Fail<T>(error ?? e.Message);
            }
        }

        public static Result<None> OfAction(Action f, string error = null)
        {
            try
            {
                f();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail<None>(error ?? e.Message);
            }
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, TOutput> continuation)
        {
            return input.Then(inp => Of(() => continuation(inp)));
        }

        public static Result<TInput> Apply<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.IsSuccess
                ? input.Then(inp =>
                {
                    continuation(inp);
                    return inp;
                })
                : Fail<TInput>(input.Error);
        }

        public static Result<None> Then<TInput, TOutput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
        }
        
        public static Result<TInput> Validate<TInput>(this Result<TInput> input, Func<TInput, bool> predicate, string errorMessage)
        {
            if (input.IsSuccess)
                return Validate(input.Value, predicate, errorMessage);
            if(predicate(input.Value))
                return Fail<TInput>(input.Error);
            return Fail<TInput>(input.Error + "\n" + errorMessage);
        }

        public static Result<None> Then<TInput>(
            this Result<TInput> input,
            Action<TInput> continuation)
        {
            return input.Then(inp => OfAction(() => continuation(inp)));
        }

        public static Result<(T1, T2)> Then<T1, T2>(this Result<T1> input, Func<Result<T2>> addition)
        {
            if (!input.IsSuccess) return Fail<(T1, T2)>(input.Error);
            var adding = addition();
            return adding.IsSuccess ? (input.Value, adding.Value) : Fail<(T1, T2)>(adding.Error);
        }
        
        public static Result<(T1, T2, T3)> Then<T1, T2, T3>(this Result<(T1, T2)> input, Func<Result<T3>> addition)
        {
            if (!input.IsSuccess) return Fail<(T1, T2, T3)>(input.Error);
            var adding = addition();
            return adding.IsSuccess
                ? (input.Value.Item1, input.Value.Item2, adding.Value)
                : Fail<(T1, T2, T3)>(adding.Error);
        }
        
        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return input.IsSuccess
                ? continuation(input.Value)
                : Fail<TOutput>(input.Error);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess) handleError(input.Error);
            return input;
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input,
            Func<string, string> replaceError)
        {
            return input.IsSuccess ? input : Fail<TInput>(replaceError(input.Error));
        }

        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input,
            string errorMessage)
        {
            return input.ReplaceError(err => errorMessage + " " + err);
        }
    }
}