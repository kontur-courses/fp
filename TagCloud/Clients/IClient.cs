using TagCloud.App;

namespace TagCloud.Clients;

public interface IClient
{
    Result<None> Run();
}