using System.Collections.Generic;
using TagCloud.ResultMonade;

namespace TagCloud.AppConfiguration
{
    public  interface IAppConfigProvider
    {
        Result<IAppConfig> GetAppConfig(IEnumerable<string> args);
    }
}
