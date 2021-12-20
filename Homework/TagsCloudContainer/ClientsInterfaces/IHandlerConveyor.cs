using System;
using System.Collections.Generic;

namespace TagsCloudContainer.ClientsInterfaces
{
    public interface IHandlerConveyor
    {
        List<Func<string, string>> GetHandlerConveyor();
        List<string> GetUnknownHandlers();
    }
}
