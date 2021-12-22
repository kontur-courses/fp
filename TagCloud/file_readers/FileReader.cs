using System.Collections.Generic;
using System.IO;

namespace TagCloud.file_readers
{
    public class FileReader : IFileReader
    {
        public Result<List<string>> GetWords(string? filename) =>
            Result.Of(() => new StreamReader(filename), ResultErrorType.InputFileError)
                .Then(TryReadWords);

        private Result<List<string>> TryReadWords(TextReader sr)
        {
            var words = new List<string>();
            var line = sr.ReadLine();

            while (line is not null)
            {
                var result = Result.Ok(line).Then(IsCorrectWord);
                if (result.IsSuccess)
                    words.Add(line);
                else
                    return Result.Fail<List<string>>(result.Error);
                line = sr.ReadLine();
            }

            return words;
        }

        private Result<string> IsCorrectWord(string word) => 
            word.Length > 0 && !word.Contains(' ') 
                ? word
                : Result.Fail<string>(ResultErrorType.InputFileError);
    }
}