using TagsCloudContainer.Common;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class GetActivePreprocessorsAction : UiAction
    {
        public override string Category => "Preprocessors";
        public override string Name => "GetActivePreprocessors";
        public override string Description => "";

        public GetActivePreprocessorsAction(IResultHandler handler)
            : base(handler)
        {
        }

        protected override void PerformAction()
        {
            var preprocessors = PreprocessorsRegistrator.GetActivePreprocessors();
            foreach (var preprocessor in preprocessors) 
                handler.AddHandledText(preprocessor.Name);
        }
    }
}