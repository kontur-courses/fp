using System;
using System.Linq;
using TagsCloud.Interfaces;

namespace TagsCloud.Validators
{
    public class ClientValidator : IClientValidator
    {
        public ClientValidator()
        {
            AddRussianCharsInMassive();
        }

        public char[] ValidChars { get; private set; }

        public Result<string> ValidateRussianInput(string line)
        {
            var result = Validate(line, l =>
                {
                    foreach (var ch in line)
                        if (!ValidChars.Contains(ch) && ch != '\n')
                            return false;

                    return true;
                },
                "Invalid word");

            return result;
        }

        public Result<string> ValidateWrongSymbolsInPath(string line)
        {
            var wrongSymbols = new[] { '<', '>', ':', '"', '\\', '/', '|', '?', '*' };

            return Validate(line, l =>
                {
                    foreach (var ch in line)
                        if (wrongSymbols.Contains(ch))
                            return false;

                    return true;
                },
                "Wrong symbols in path name");
        }

        public Result<string> ValidateRightTextExtension(string line)
        {
            var expectedTextExtensions = new[] { ".txt" };

            return ValidateExtensions(expectedTextExtensions, line);
        }

        public Result<string> ValidateRightPictureExtension(string line)
        {
            var expectedPictureExtensions = new[] { ".png", ".jpg" };

            return ValidateExtensions(expectedPictureExtensions, line);
        }

        private Result<string> ValidateExtensions(string[] availableExtensions, string line)
        {
            return Validate(line, l =>
                {
                    var extInd = line.LastIndexOf('.');
                    var ext = "";

                    if (extInd != -1) ext = line.Substring(extInd);
                    else return false;

                    if (availableExtensions.Contains(ext)) return true;
                    return false;
                },
                $"Wrong extension. Expected only {string.Join(" ", availableExtensions)} files");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            return predicate(obj)
                ? Result<T>.Ok(obj)
                : Result<T>.Fail<T>(errorMessage);
        }

        private void AddRussianCharsInMassive()
        {
            char ch;
            var n = 0;
            ValidChars = new char[64];
            for (var i = 1040; i <= 1103; i++)
            {
                ch = Convert.ToChar(i);
                ValidChars[n] = ch;
                n++;
            }
        }
    }
}