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

        public Result<IEnumerable<string>> ReadFrom(string path)
        {
            return Result.Of(() => ReadUnsafe(path));
        }

        public IEnumerable<string> ReadUnsafe(string path)
        {
            using (var r = new StreamReader(path))
            {
                return (string[]) serializer.Deserialize(r);
            }
        }
    }
}