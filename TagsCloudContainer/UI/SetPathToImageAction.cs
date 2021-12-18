using System;
using System.Text.RegularExpressions;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class SetPathToImageAction : UiAction
    {
        public override string Category => "AppSettings";
        public override string Name => "SetPathToImage";
        public override string Description => "";

        public SetPathToImageAction(IResultHandler handler)
            :base(handler)
        {
        }

        protected override void PerformAction()
        {
            var path = handler.GetText();
            var r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            if (!r.IsMatch(path)) 
                throw new Exception("It`s not good path to file, Check it for mistakes");
            AppSettings.ImageFilename = path;
        }
    }
}