using System;
using System.IO;
using ResultOf;

namespace TagCloud.Infrastructure
{
    public class FileInfoProvider : IFileInfoProvider
    {
        public Result<FileInfo> GetFileInfo(string path)
        {
            if (Directory.Exists(path))
                return Result.Fail<FileInfo>($"{path} should be a file");
            var fileDirectory = Directory.GetParent(path);
            if (!fileDirectory.Exists)
                return Result.Fail<FileInfo>($"Directory {fileDirectory.FullName} doesn't exist");
            if (!File.Exists(path))
                return Result.Fail<FileInfo>($"File {path} doesn't exist");
            return new FileInfo(path);
        }
    }
}