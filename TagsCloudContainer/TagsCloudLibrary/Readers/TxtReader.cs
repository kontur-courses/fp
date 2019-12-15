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

        public Result<Stream> Read(Stream stream)
        {
            return Result.Ok(stream);
        }
    }
}
