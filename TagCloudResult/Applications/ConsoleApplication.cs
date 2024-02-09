using TagCloudResult.Drawer;

namespace TagCloudResult.Applications
{
    public class ConsoleApplication(IDrawer drawer, Settings settings) : IApplication
    {
        public Result Run()
        {
            var imageResult = drawer.GetImage();
            if (!imageResult.IsSuccess)
                return imageResult.Fail();

            imageResult.Value.Save($"{settings.SavePath}.{settings.ImageFormat.ToLower()}", settings.GetFormat());
            Console.WriteLine($"Saved to {settings.SavePath + '.' + settings.ImageFormat.ToLower()}");

            return Result.Ok();
        }
    }
}
