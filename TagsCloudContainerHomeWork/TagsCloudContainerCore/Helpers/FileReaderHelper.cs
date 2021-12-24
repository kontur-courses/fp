using System;
using System.Collections.Generic;
using System.IO;
using TagsCloudContainerCore.Result;

namespace TagsCloudContainerCore.Helpers;

// ReSharper disable once UnusedType.Global
public static class FileReaderHelper
{
    // ReSharper disable once UnusedMember.Global
    public static Result<IEnumerable<string>> ReadLinesFromFile(string path, bool ignoreNotFileExistExc = false)
    {
        if (ignoreNotFileExistExc && !File.Exists(path))
        {
            return ArraySegment<string>.Empty;
        }

        try
        {
            var result = new List<string>();
            using var fileRider = new StreamReader(path);
            var line = fileRider.ReadLine();

            while (line is not null)
            {
                result.Add(line);
                line = fileRider.ReadLine();
            }

            return result;
        }
        catch (Exception e)
        {
            return ResultExtension.Fail<IEnumerable<string>>($"Ошибка при чтении файла{path}\n:" +
                                                             $"{e.GetType().Name}: {e.Message}");
        }
    }
}