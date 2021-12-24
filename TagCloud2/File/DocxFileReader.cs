using ResultOf;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace TagCloud2.Text
{
    public class DocxFileReader : IFileReader
    {
        public Result<string> ReadFile(string path)
        {
            const string wordmlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            if (!File.Exists(path))
            {
                return Result.Fail<string>("No such file to open");
            }

            using var file = File.OpenRead(path);
            try
            {
                using var zip = new ZipArchive(file, ZipArchiveMode.Read);
                var SB = new StringBuilder();
                var entry = zip.Entries.First(x => x.Name == "document.xml");
                var nt = new NameTable();
                var nsManager = new XmlNamespaceManager(nt);
                nsManager.AddNamespace("w", wordmlNamespace);
                var xml = new XmlDocument(nt);
                using var doc = entry.Open();
                xml.Load(doc);
                var nodes = xml.SelectNodes("//w:p", nsManager);
                foreach (XmlNode node in nodes)
                {
                    SB.Append(node.InnerText + Environment.NewLine);
                }

                return Result.Ok(SB.ToString());
            }
            catch
            {
                return Result.Fail<string>("Something is wrong with docx file");
            }
        }
    }
}
