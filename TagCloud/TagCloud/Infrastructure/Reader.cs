using ResultOF;
using System.IO;
using System.Text.RegularExpressions;

namespace TagCloud
{
    public class Reader
    {
        public readonly DocReader docReader;
        public readonly TxtReader txtReader;

        public Reader(DocReader docReader, TxtReader txtReader)
        {
            this.docReader = docReader;
            this.txtReader = txtReader;
        }

        public Result<string> ReadTextFromFile()
        {
            if (PathToFile == null)
                return Result.Fail<string>("Get words to read first!");
            if (Regex.IsMatch(PathToFile, @"\w*.doc$"))
                return docReader.ReadAllText(PathToFile);
            else if (Regex.IsMatch(PathToFile, @"\w*.txt$"))
                return txtReader.ReadAllText(PathToFile);
            else
                return Result.Fail<string>("No reader for this file");
        }

        public string PathToFile { get; set; }

    }
}
