using System.IO;
using TagsCloudContainer.Settings;

namespace TagsCloudContainer.FileReaders
{
    public class TextFileReader : IFileReader
    {
        private readonly FileSettings fileSettings;

        public TextFileReader(FileSettings fileSettings)
        {
            this.fileSettings = fileSettings;
        }

        public Result<string> Read()
        {
            if (!File.Exists(fileSettings.Filename))
                return Result.Fail<string>($"file not found: {fileSettings.Filename}");
            var textFromFile = File.ReadAllText(fileSettings.Filename);
            return Result.Ok(textFromFile);
        }
    }
}