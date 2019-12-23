using System;
using Autofac;
using TagsCloud.ErrorHandler;

namespace TagsCloud
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Result.Ok(args)
                .Then(Options.Parse)
                .Then(ContainerConstructor.Configure)
                .Then(c => c.Resolve<Application>())
                .Then(a => a.Run())
                .OnFail(Console.WriteLine);
        }
    }
}