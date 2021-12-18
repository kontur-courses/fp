using System;
using System.IO;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class SetPathToTagsTextAction : UiAction
    {
        public override string Category => "AppSettings";
        public override string Name => "SetPathToTagsText";
        public override string Description => "";

        public SetPathToTagsTextAction(IResultHandler handler)
            : base(handler)
        {
        }

        protected override void PerformAction()
        {
            var path = handler.GetText();
            if (!File.Exists(path)) 
                throw new Exception("There`re no file by this path, Check it for mistakes");
            AppSettings.TextFilename = path;
        }
    }
}