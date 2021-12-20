using System;
using System.Drawing.Imaging;

namespace TagsCloudVisualizationDI.Saving
{
    public class DefaultSaver : ISaver
    {
        private string SavePath { get; }

        public DefaultSaver(string savePath, ImageFormat imageFormat)
        {
            SavePath = savePath + '.' + imageFormat;
        }

        public Result<string> GetSavePath()
        {
            Checker.CheckPathToDirectory(SavePath
                .Substring(0, SavePath.LastIndexOf("\\", StringComparison.InvariantCulture)+1));
            return SavePath;
        }
    }
}
