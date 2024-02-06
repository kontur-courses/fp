using Microsoft.Extensions.DependencyInjection;
using TagsCloud.CustomAttributes;
using TagsCloud.Formatters;

namespace TagsCloud.FileReaders;

[Injection(ServiceLifetime.Singleton)]
public class TxtFileReader : IFileReader
{
    public string SupportedExtension => "txt";

    public IEnumerable<string> ReadContent(string filename, IPostFormatter postFormatter = null)
    {
        return File
               .ReadLines(filename)
               .Select(line => postFormatter is null ? line : postFormatter.Format(line));
    }
}