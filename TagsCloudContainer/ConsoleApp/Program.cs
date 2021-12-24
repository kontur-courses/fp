using TagsCloudVisualization.ResultOf;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            Result.OfAction(client.CreateCloud)
                .RefineError("Something went wrong")
                .GetValueOrThrow(); 
        }
    }
}