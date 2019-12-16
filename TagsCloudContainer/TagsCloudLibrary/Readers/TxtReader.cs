using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Readers
{
    public class TxtReader : IReader
    {
        public string Name { get; } = "txt";

        public Result<Stream> Read(string fileName)
        {
            try
            {
                return Result.Ok<Stream>(File.OpenRead(fileName));
            }
            catch (Exception e)
            {
                return Result.Failure<Stream>(e.Message);
            }
        }
    }
}
