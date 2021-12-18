using Autofac;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public abstract class UiAction : IUiAction
    {
        protected readonly ContainerBuilder builder;
        protected readonly IResultHandler handler;
        public abstract string Category { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public UiAction(IResultHandler handler)
        {
            this.handler = handler;
        }
        public UiAction(IResultHandler handler, ContainerBuilder builder)
            : this(handler)
        {
            this.builder = builder;
        }

        public void Perform()
        {
            handler.AddHandledText(Description);
            handler.Handle(PerformAction);
        }

        protected abstract void PerformAction();
    }
}