using System.IO;
using TagsCloudContainer.Enums;
using TagsCloudContainer.Interfaces;
using TagsCloudContainer.Readers;
using TagsCloudContainer.Utility;

namespace TagsCloudContainer.TagsCloud
{
    public class FileReader
    {
        public Result<string[]> ReadFile(string filePath)
        {
            try
            {
                var fileReaderResult = GetFileReader(filePath).ReadWords(filePath);

                return fileReaderResult.IsSuccess
                    ? Result.Ok(fileReaderResult.Value.ToArray())
                    : Result.Fail<string[]>(fileReaderResult.Error);
            }
            catch (Exception e)
            {
                return Result.Fail<string[]>($"Error reading file: {e.Message}");
            }
        }

        private IFileReader GetFileReader(string filePath)
        {
            return GetFileType(filePath).Then(fileType =>
            {
                switch (fileType)
                {
                    case FileType.Doc:
                        return Result.Ok<IFileReader>(new DocReader());
                    case FileType.Docx:
                        return Result.Ok<IFileReader>(new DocxReader());
                    case FileType.Txt:
                        return Result.Ok<IFileReader>(new TxtReader());
                    default:
                        return Result.Fail<IFileReader>("Unsupported file extension");
                }
            }).GetValueOrThrow();
        }

        private Result<FileType> GetFileType(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath)?.ToLower();

            switch (fileExtension)
            {
                case ".doc":
                    return Result.Ok(FileType.Doc);
                case ".docx":
                    return Result.Ok(FileType.Docx);
                case ".txt":
                    return Result.Ok(FileType.Txt);
                default:
                    return Result.Fail<FileType>("Unsupported file extension");
            }
        }
    }
}
