using ResultOF;

namespace TagCloud
{
    public  interface IReader
    {
        Result<string> ReadAllText(string pathToFile);
    }
}
