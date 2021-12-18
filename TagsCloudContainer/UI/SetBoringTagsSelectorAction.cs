using System;
using Autofac;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using TagsCloudContainer.Common.Result;
using TagsCloudContainer.Preprocessors;

namespace TagsCloudContainer.UI
{
    public class SetBoringTagsSelectorAction : UiAction
    {
        public override string Category => "Preprocessors";
        public override string Name => "SetBoringTagsSelector";
        public override string Description => "Enter selector to choose all 'good' tags\n" +
                                              "Don`t forget to activate CustomTagsFilter!";

        public SetBoringTagsSelectorAction(IResultHandler handler, ContainerBuilder builder)
            : base(handler, builder)
        {
        }

        protected override void PerformAction()
        {
            var selector = handler.GetText();
            try
            {
                var t = CSharpScript.EvaluateAsync<CustomTagsFilter.RelevantTag>(selector);
                t.Wait();
                builder.RegisterInstance(t.Result)
                    .As<CustomTagsFilter.RelevantTag>();
            }
            catch (Exception)
            {
                throw new Exception("It wasn`t selector\n" +
                                    "It should be Func<SimpleTag, bool> like that:\n" +
                                    "t => t.Word.Length < 10");
            }
        }
    }
}