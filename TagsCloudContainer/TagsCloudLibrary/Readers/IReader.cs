using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;

namespace TagsCloudLibrary.Readers
{
    public interface IReader
    {
        string Name { get; }
        Result<Stream> Read(string fileName);
    }
}
