using TagCloudContainer.TaskResult;

namespace TagCloudContainer.FileReaders
{
    /// <summary>
    ///  Нужен чтобы можно было реализовать открытие разных форматов
    /// </summary>
    public interface IFileReader
    {
        public Result<string[]> FileToWordsArray(string filePath);
    }
}