﻿using ResultOf;
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
                var entry = zip.Entries.Where(x => x.Name == "document.xml").First();
                NameTable nt = new();
                XmlNamespaceManager nsManager = new(nt);
                nsManager.AddNamespace("w", wordmlNamespace);
                var xml = new XmlDocument(nt);
                using var doc = entry.Open();
                xml.Load(doc);
                var nodes = xml.SelectNodes("//w:p", nsManager);
                foreach (XmlNode node in nodes)
                {
                    SB.Append(node.InnerText + Environment.NewLine);
                }

                return Result.Ok<string>(SB.ToString());
            }
            catch
            {
                return Result.Fail<string>("Something is wrong with docx file");
            }
        }
    }
}
