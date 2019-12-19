using System;
using System.Linq;
using CommandLine;
using TagsCloudContainer.Models;

namespace TagsCloudContainer
{
    public class ConsoleUserHandler : IUserHandler
    {
        private readonly string[] args;

        public ConsoleUserHandler(string[] args)
        {
            this.args = args;
        }

        public Result<InputInfo> GetInputInfo()
        {
            Result<InputInfo> inputInfoResult = new InputInfo();
            var a = Parser.Default.ParseArguments<StandartOptions>(args)
                    .WithParsed(opts => inputInfoResult = new InputInfo(opts.File, opts.MaxCnt, opts.Format))
                    .WithNotParsed(opts => inputInfoResult = Result.Fail<InputInfo>(opts.First().ToString()));
            return inputInfoResult;
        }
    }
}