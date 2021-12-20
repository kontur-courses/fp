using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.ClientsInterfaces;

namespace CLI
{
    public class CommandLineHandlerConveyor : IHandlerConveyor
    {
        private readonly List<Func<string, string>> wordHandlers;
        private readonly List<string> unknownHandlers;

        public CommandLineHandlerConveyor(List<Func<string, string>> existedHandlers,
            IEnumerable<string> userHandlers)
        {
            wordHandlers = existedHandlers;
            unknownHandlers = userHandlers.ToList();
        }

        public List<Func<string, string>> GetHandlerConveyor()
            => wordHandlers;

        public List<string> GetUnknownHandlers()
            => unknownHandlers;
    }
}
