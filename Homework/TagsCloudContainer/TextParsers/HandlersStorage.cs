using System;
using System.Collections.Generic;

namespace TagsCloudContainer.TextParsers
{
    public class HandlersStorage
    {
        private static readonly Dictionary<string, Func<string, string>> allHandlers;
        private readonly List<Func<string, string>> usingHandlers;
        private readonly List<string> unrecognizedHandlers;

        static HandlersStorage()
        {
            allHandlers = new Dictionary<string, Func<string, string>>
            {
                {"lower", s => s.ToLower()},
                {"trim", s => s.Trim()}
            };
        }

        public HandlersStorage(IEnumerable<string> userHandlers)
        {
            usingHandlers = new List<Func<string, string>>();
            unrecognizedHandlers = new List<string>();
            RecognizeUserHandlers(userHandlers);
        }

        public void AddCustomHandler(Func<string, string> customHandler)
            => usingHandlers.Add(customHandler);

        private void RecognizeUserHandlers(IEnumerable<string> userHandlers)
        {
            foreach (var handler in userHandlers)
            {
                if (allHandlers.ContainsKey(handler)) usingHandlers.Add(allHandlers[handler]);
                else unrecognizedHandlers.Add(handler);
            }
        }

        public IReadOnlyList<Func<string, string>> GetUsingHandlers()
            => usingHandlers;

        public IReadOnlyList<string> GetUnrecognizedHandlers()
            => unrecognizedHandlers;
    }
}
