using System;
using CTV.Common.Readers;

namespace CTV.Common.VisualizerProcessorFactory
{
    public static class InputFileFormatExtensions
    {
        public static IFileReader ToFileReader(this InputFileFormat format)
            => format switch
            {
                InputFileFormat.Txt => new TxtFileReader(),
                InputFileFormat.Doc => new DocFileReader(),
                _ => throw new InvalidOperationException($"Can not find file reader for {format}")
            };
    }
}