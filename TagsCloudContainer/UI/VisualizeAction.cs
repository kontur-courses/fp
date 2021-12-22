using System;
using Autofac;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class VisualizeAction : UiAction
    {
        public override string Category => "Visualization";
        public override string Name => "Visualize";
        public override string Description => "Visualize with current settings, yes or no? 'y', 'n'";

        public VisualizeAction (IResultHandler handler, ContainerBuilder builder)
            :base(handler, builder)
        {
        }

        protected override void PerformAction()
        {
            var answer = handler.GetText();
            switch (answer)
            {
                case "y":
                    var container = builder.Build();
                    var processor = container.Resolve<Processor>();
                    processor.Process();
                    Environment.Exit(0);
                    return;
                case "n":
                    return;
                default:
                    throw new Exception("Answer should be 'y' or 'n'");
            }
        }
    }
}