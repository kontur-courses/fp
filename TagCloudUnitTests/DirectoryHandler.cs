using System.IO;
using System;
using System.Linq;

namespace TagCloudUnitTests
{
    public static class DirectoryHandler
    {
        public static DirectoryInfo GetSolutionDirectory()
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (baseDirectory != null && !baseDirectory.GetFiles("*.sln").Any())
            {
                baseDirectory = baseDirectory.Parent;
            }

            return baseDirectory;
        }
    }
}
