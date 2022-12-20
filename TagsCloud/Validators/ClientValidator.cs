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

        public ResultHandler<string> ValidateRussianInput(string line)
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

        public ResultHandler<string> ValidateWrongSymbolsInFileName(string line)
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

        public ResultHandler<string> ValidateRightTextExtension(string line)
        {
            var expectedTextExtensions = new[] { ".txt" };

            return ValidateExtensions(expectedTextExtensions, line);
        }

        public ResultHandler<string> ValidateRightPictureExtension(string line)
        {
            var expectedPictureExtensions = new[] { ".png", ".jpg" };

            return ValidateExtensions(expectedPictureExtensions, line);
        }

        private ResultHandler<string> ValidateExtensions(string[] availableExtensions, string line)
        {
            var isDotInLine = false;

            var res = Validate(line, l =>
                {
                    var extInd = line.LastIndexOf('.');
                    var ext = "";

                    if (extInd != -1) ext = line.Substring(extInd);
                    else
                    {
                        isDotInLine = true;
                        return false;
                    }

                    if (availableExtensions.Contains(ext)) return true;
                    return false;
                },
                $"Wrong extension. Expected only {string.Join(" ", availableExtensions)} files");

            if (isDotInLine) res.RefineError("A dot in the extension name is not found");
            return res;
        }


        private ResultHandler<T> Validate<T>(T obj, Func<T, bool> predicate, string errorMessage)
        {
            var handler = new ResultHandler<T>(obj);

            return predicate(obj)
                ? handler
                : handler.Fail(errorMessage);
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