using System.IO;
using App.Infrastructure.FileInteractions.Readers;
using App.Infrastructure.SettingsHolders;

namespace App.Implementation.FileInteractions.Readers
{
    public class ReaderFactory : IReaderFactory
    {
        private readonly IInputFileSettingsHolder inputFileSettings;

        public ReaderFactory(IInputFileSettingsHolder inputFileSettings)
        {
            this.inputFileSettings = inputFileSettings;
        }

        public Result<ILinesReader> CreateReader()
        {
            return Path.GetExtension(inputFileSettings.InputFileName) switch
            {
                ".txt" => new Result<ILinesReader>(
                    null,
                    new FromStreamReader(new StreamReader(inputFileSettings.InputFileName))),

                ".doc" or ".docx" => new Result<ILinesReader>(
                    null,
                    new FromDocReader(inputFileSettings.InputFileName)),
                _ => Result.Fail<ILinesReader>($"Unknown extension of file {inputFileSettings.InputFileName}")
            };
        }
    }
}