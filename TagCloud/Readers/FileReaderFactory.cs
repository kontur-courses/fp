using System.Collections.Generic;
using System.Linq;
using TagCloud.ResultMonad;

namespace TagCloud.Readers
{
    public class FileReaderFactory : IFileReaderFactory
    {
        private static readonly Dictionary<string, IFileReader> fileReadersFactory =
            new()
            {
                {"txt", new TextReader()},
                {"xml", new XmlFileReader()},
                {"docx", new DocFileReader()}
            };
        
        public Result<IFileReader> Create(string fileExtension)
        {
            //var fileExtension = GetExtensionsFromFileName(filename);

            if (!fileReadersFactory.TryGetValue(fileExtension, out var reader))
                return Result.Fail<IFileReader>("The tag cloud generator does not " +
                                                "support the input file with the extension " +
                                                $"\"{fileExtension}\".\n" +
                                                "Please make an introductory file with one of these extensions:" +
                                                string.Join(" ", fileReadersFactory.Keys.ToArray()));
            return reader.AsResult();
        }
    }
}
