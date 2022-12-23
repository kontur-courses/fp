using TagCloud.ResultMonade;

namespace TagCloud.App
{
    public interface IApp
    {
        Result<None> Run();
    }
}
