using TagCloudResult.Drawer;

namespace TagCloudResult.Applications
{
    public class ConsoleApplication(IDrawer drawer, Settings settings) : IApplication
    {
        public void Run()
        {
            var imageResult = drawer.GetImage();
            if (!imageResult.IsSuccess)
            {
                Console.WriteLine(imageResult.Error);
                return;
            }

            imageResult.Value.Save($"{settings.SavePath}.{settings.ImageFormat.ToLower()}", settings.GetFormat());
            Console.WriteLine($"Saved to {settings.SavePath + '.' + settings.ImageFormat.ToLower()}");
        }
    }
}
