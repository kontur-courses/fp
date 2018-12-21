using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer;
using TagsCloudResult;

namespace TagsCloudBuilder.Drawer
{
    public class DebugDrawer : IDrawer
    {
        private readonly Size canvasSize;
        private readonly List<ContainerInfo> containers;
        private readonly string fileName;
        private readonly ImageFormat imageFormat;

        public DebugDrawer(IContainersCreator containersCreator,
            Size canvasSize,
            string fileName,
            ImageFormat imageFormat)
        {
            this.canvasSize = canvasSize;
            this.fileName = fileName;
            containers = containersCreator.ContainersInfo;
            this.imageFormat = imageFormat;
        }

        public Result<None> DrawAndSaveWords()
        {
            var stringFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            using (var bitmap = new Bitmap(canvasSize.Height, canvasSize.Width))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.FillRectangle(Brushes.White, 0, 0, canvasSize.Height, canvasSize.Width);
                    foreach (var container in containers)
                    {
                        graphics.DrawRectangle(Pens.Black, container.Rectangle);
                        graphics.DrawString(
                            container.Text,
                            container.TextFont,
                            new SolidBrush(container.TextColor),
                            container.Rectangle,
                            stringFormat);
                    }

                    if (Result.OfAction(() => bitmap.Save(fileName, imageFormat)).IsSuccess)
                        return Result.Ok();

                    return Result.Fail<None>(
                        $"Can't create file with this path(name): {fileName}. Check the correctness of file path.");
                }
            }
        }
    }
}
