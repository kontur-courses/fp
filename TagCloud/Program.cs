using TagCloud.UI.Console;


namespace TagCloud
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //using var container = DependencyConfigurator.GetConfiguredContainer();
            var client = new ConsoleUI();
            client.Run(args);
        }
    }
}
