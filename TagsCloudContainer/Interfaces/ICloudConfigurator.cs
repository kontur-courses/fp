using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ICloudConfigurator
    {
        Result<IEnumerable<KeyValuePair<string, int>>> ConfigureCloud();
    }
}
