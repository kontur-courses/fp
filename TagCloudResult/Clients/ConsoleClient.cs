using TagCloudResult.Savers;

namespace TagCloudResult.Clients;

public class ConsoleClient : Client
{
    public ConsoleClient(CloudLayouter layouter, CloudDrawer drawer, IBitmapSaver saver) : base(layouter, drawer, saver)
    {
    }

    public override void PrintError(string error)
    {
        Console.WriteLine(error);
        Environment.Exit(1);
    }
}