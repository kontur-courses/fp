using Autofac;
using TagCloud.Utility.Container;

namespace TagCloud.Tests
{
    public class TestBase
    {
        protected readonly IContainer container = ContainerConfig.StandartContainer;
    }
}