﻿using System;

namespace ResultOfTask
{
    public class None
    {
        private None()
        {
        }
    }

    public struct Result<T>
    {
        public Result(string error, T value = default(T))
        {
            Error = error;
            Value = value;
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

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(null, value);
        }
        
        public static Result<None> Ok()
        {
            return new Result<None>(null);
        }

        public static Result<T> Fail<T>(string e)
        {
            return new Result<T>(e);
        }
        
        public static Result<None> Fail(string e)
        {
            return new Result<None>(e);
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
        
        public static Result<None> Of(Action f, string error = null)
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
            return Then(input, (i) => Of(() => continuation(i)));
        }

        public static Result<TOutput> Then<TInput, TOutput>(
            this Result<TInput> input,
            Func<TInput, Result<TOutput>> continuation)
        {
            return !input.IsSuccess
                ? Fail<TOutput>(input.Error)
                : continuation(input.Value);
        }

        public static Result<TInput> OnFail<TInput>(
            this Result<TInput> input,
            Action<string> handleError)
        {
            if (!input.IsSuccess)
                handleError(input.Error);
            return input;
        }
        
        public static Result<TInput> RefineError<TInput>(
            this Result<TInput> input, 
            string newError)
        {
            return input.IsSuccess 
                ? input
                : new Result<TInput>($"{newError}. {input.Error}");
        }

        public static Result<TInput> ReplaceError<TInput>(
            this Result<TInput> input, 
            Func<Result<TInput>, string> func)
        {
            return input.IsSuccess 
                ? input
                : new Result<TInput>(func(input));
        }
    }
}