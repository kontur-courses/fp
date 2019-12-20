using System;
using System.IO;
using TagsCloudVisualization.Results;

namespace TagsCloudVisualization
{
    internal class PathHelper
    {
        public static Result<string> ResourcesPath
        {
            get
            {
                var pathToResources = Environment.CurrentDirectory;

                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        pathToResources = Directory.GetParent(pathToResources).FullName;
                    }
                    catch (Exception e)
                    {
                        return Result.Fail<string>($"Cant get parent directory for {pathToResources} " + e.Message);
                    }
                }

                pathToResources += "\\Resources";
                return pathToResources;
            }
        }
    }
}