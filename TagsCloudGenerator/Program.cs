using TagsCloudGenerator.Client;
using TagsCloudGenerator.Client.Console;

namespace TagsCloudGenerator
{
    internal class Program
    {
        private static void Main()
        {
            var client = GetClient();

            client.Run();
        }

        private static IClient GetClient()
        {
            return new ConsoleClient();
        }
    }
}