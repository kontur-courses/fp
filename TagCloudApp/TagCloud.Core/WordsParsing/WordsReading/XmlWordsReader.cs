using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using TagCloud.Core.Util;

namespace TagCloud.Core.WordsParsing.WordsReading
{
    public class XmlWordsReader : IWordsReader
    {
        private readonly XmlSerializer serializer;

        public XmlWordsReader()
        {
            AllowedFileExtension = new Regex(@"\.xml$", RegexOptions.IgnoreCase);
            serializer = new XmlSerializer(typeof(string[]));
        }

        public Regex AllowedFileExtension { get; }

        public Result<IEnumerable<string>> ReadFrom(Stream stream)
        {
            return Result.Of(() => ReadUnsafeFrom(stream));
        }

        private IEnumerable<string> ReadUnsafeFrom(Stream stream)
        {
            return (string[]) serializer.Deserialize(stream);
        }
    }
}