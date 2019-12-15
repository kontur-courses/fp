using System.IO;
using System.Reflection;

namespace TagsCloudGeneratorExtensions
{
    internal static class Metadata
    {
        public static string PathToMyStem =>
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Resources",
                "mystem.exe");
    }
}