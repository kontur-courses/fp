using System;

namespace CTV.Common.Readers
{
    public static class FileReaderFactory
    {
        public static IFileReader ToFileReader(this InputFileFormat format)
            => format switch
            {
                InputFileFormat.Doc => new DocFileReader(),
                InputFileFormat.Txt => new TxtFileReader(),
                _ => throw new Exception("Unknown input file format")
            };
    }
}