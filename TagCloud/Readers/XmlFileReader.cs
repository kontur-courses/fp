using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TagCloud.Readers
{
    public class XmlFileReader : IFileReader
    {
        public Result<string[]> ReadFile(string filename)
        {
            var doc = new XmlDocument();
            var text = new List<string>();
            var loadResult = Result.OfAction(() => doc.Load(filename));
            if (!loadResult.IsSuccess)
                return Result.Fail<string[]>($"File {filename} not found." +
                                             $" Please check that file exists");
            if (doc.DocumentElement == null)
                return Result.Fail<string[]>("Invalid content format");

            if (doc.DocumentElement.Name != "words")
                return Result.Fail<string[]>("Xml root should called \"words\"");

            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
                text.Add(node.InnerText);

            return text.ToArray().AsResult();
        }
    }
}
