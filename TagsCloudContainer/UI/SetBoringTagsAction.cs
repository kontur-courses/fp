using System;
using System.Linq;
using Autofac;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class SetBoringTagsAction : UiAction
    {
        public override string Category => "Preprocessors";
        public override string Name => "SetBoringTags";

        public override string Description => "Enter all words, you don`t want to see in cloud," +
                                              "splitted by whiteSpace\n" +
                                              "Don`t forget to activate BoringTagsFilter!";

        public SetBoringTagsAction(IResultHandler handler, ContainerBuilder builder)
            : base(handler, builder)
        {
        }

        protected override void PerformAction()
        {
            var tags = handler.GetText()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToHashSet();
            builder.RegisterInstance(tags).AsSelf();
        }
    }
}