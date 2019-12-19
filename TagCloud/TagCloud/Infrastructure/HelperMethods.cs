using ResultOF;
using System;
using System.IO;

namespace TagCloud
{
    public static class HelperMethods
    {
        public static Result<string> GetProjectDirectory()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            return projectDirectory;
        }
    }
}
