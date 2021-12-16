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
            switch (Path.GetExtension(inputFileSettings.InputFileName))
            {
                case ".txt":
                    return new Result<ILinesReader>(
                        null,
                        new FromStreamReader(new StreamReader(inputFileSettings.InputFileName)));
                case ".doc":
                case ".docx":
                    return new Result<ILinesReader>(
                        null,
                        new FromDocReader(inputFileSettings.InputFileName));
            }

            return Result.Fail<ILinesReader>("Unknown input file extension");
        }
    }
}