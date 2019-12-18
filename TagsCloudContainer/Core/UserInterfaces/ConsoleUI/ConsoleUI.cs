using System.Collections.Generic;
using CommandLine;

namespace TagsCloudContainer.Core.UserInterfaces.ConsoleUI
{
    class ConsoleUi : IUi
    {
        private readonly ApplicationCore applicationCore;

        public ConsoleUi(ApplicationCore applicationCore)
        {
            this.applicationCore = applicationCore;
        }

        public void Run(IEnumerable<string> userInput)
        {
            Parser.Default
                .ParseArguments<Options>(userInput)
                .WithParsed(applicationCore.Run);
        }
    }
}