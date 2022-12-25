using FluentAssertions;
using TagsCloudVisualization.Clients;

namespace Testing;

[TestFixture]
public class ClientTests
{
    private ConsoleClient client;

    [SetUp]
    public void SetUp()
    {
        client = new ConsoleClient();
    }

    [TestCase("-c zxccvb")]
    [TestCase("-h h")]
    public void IncorrectOptionsTest(string args)
    {
        var consoleOutput = GetConsoleOutput(args);
        consoleOutput
            .Should()
            .Contain("Options error");
    }

    [TestCase("-w 1 -h 1")]
    [TestCase("-s 800")]
    public void AllTextInCloudTest(string args)
    {
        var consoleOutput = GetConsoleOutput(args);
        consoleOutput
            .Should()
            .Contain("Drawing error");
    }

    [TestCase("-p C:\\input.txt")]
    public void InvalidPathTest(string args)
    {
        var consoleOutput = GetConsoleOutput(args);
        consoleOutput
            .Should()
            .Contain("Input error");
    }

    public string GetConsoleOutput(string args)
    {
        using (StringWriter stringWriter = new StringWriter())
        {
            Console.SetOut(stringWriter);

            client.Run(args.Split());

            return stringWriter.ToString();
        }
    }
}