using System;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainerCore.Helpers;

// ReSharper disable once UnusedType.Global
public static class FileReaderHelper
{
    // ReSharper disable once UnusedMember.Global
    public static IEnumerable<string> ReadLinesFromFile(string path, bool ignoreNotFileExistExc = false)
    {
        if (ignoreNotFileExistExc && !File.Exists(path))
        {
            return ArraySegment<string>.Empty;
        }

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
}