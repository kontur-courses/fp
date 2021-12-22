using System;
using TagsCloud.Visualization.Utils;
using Xceed.Words.NET;

namespace TagsCloud.Visualization.TextProviders.FileReaders
{
    public class DocFileReader : IFileReader
    {
        public Result<string> Read(string filename)
        {
            try
            {
                using var document = DocX.Load(filename);
                return document.Text;
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }
        }

        public bool CanRead(string extension) => extension == "docx" || extension == "doc";
    }
}