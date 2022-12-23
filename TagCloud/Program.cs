using System;
using Autofac;
using TagCloud.App;
using TagCloud.AppConfiguration;
using TagCloud.ResultMonade;

namespace TagCloud
{
    public class Program
    {
        static void Main(string[] args)
        {         
            new ConsoleAppConfigProvider().GetAppConfig(args)
                                          .Then(config => ContainerConfig.Configure(config))
                                          .Then(container => container.Resolve<IApp>())
                                          .Then(app => app.Run())
                                          .RefineError("OMG! Application failed")
                                          .OnFail(Console.WriteLine);
        }
    }
}

