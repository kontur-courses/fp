using TagCloud.UI.Console;


namespace TagCloud
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var client = new ConsoleUI();
            
            client.Run(args);
        }
    }
}
