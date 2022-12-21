using System;
using System.Collections.Generic;
using System.IO;

namespace TagCloud.Readers
{
    public class SingleWordInRowTextFileReader : IBoringWordsReader
    {
        private Result<string> path;
        private readonly IEnumerable<string> defaultWords = "У меня нет слов".Split();
        public string FileExtFilter => "txt files (*.txt)|*.txt";

        public void SetFile(string path)
        {
            this.path = Result.Ok(path)
                .Then(CheckNullArgument)
                .Then(CheckExistenceOfFile);
        }


        private Result<string> CheckNullArgument(string path) =>
            Validate(path, p => string.IsNullOrWhiteSpace(p), $"path is not defined");

        private Result<string> CheckExistenceOfFile(string path) =>
            Validate(path, p => !File.Exists(p), $"file {path} not found");

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage) =>
            predicate(obj)
                ? Result.Ok(obj)
                : Result.Fail<T>(errorMessage);

        public Result<IEnumerable<string>> ReadWords() =>
            !path.IsSuccess 
                ? Result.Fail<IEnumerable<string>>(path.Error) 
                : Result.Ok(path.Value == null 
                    ? defaultWords 
                    : File.ReadAllLines(path.Value));
    }
}
