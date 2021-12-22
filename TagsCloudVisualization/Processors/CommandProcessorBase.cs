using Autofac;

namespace TagsCloudVisualization.Processors
{
    public class CommandProcessorBase<T>
    {
        protected readonly IContainer container;

        protected CommandProcessorBase()
        {
            container = ContainerConfig.ConfigureContainer().GetValueOrThrow();
        }
        
        public virtual int Run(T options)
        {
            return 0;
        }
    }
}