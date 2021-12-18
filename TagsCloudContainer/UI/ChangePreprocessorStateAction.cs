using System;
using System.Linq;
using TagsCloudContainer.Common;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class ChangePreprocessorStateAction : UiAction
    {
        public override string Category => "Preprocessors";
        public override string Name => "ChangePreprocessorState";
        public override string Description => "Enter Preprocessor name";

        public ChangePreprocessorStateAction(IResultHandler handler)
            : base(handler)
        {
        }

        protected override void PerformAction()
        {
                var processorName = handler.GetText();
                var userPreprocessor = PreprocessorsRegistrator.GetActivePreprocessors()
                    .FirstOrDefault(t => t.Name == processorName);
                if (userPreprocessor == null)
                    throw new Exception("Preprocessor with that name not found, " +
                                        "check existing preprocessors by command GetAllPreprocessors");
                ChangeState(userPreprocessor);
        }

        private void ChangeState(Type preprocessorType)
        {
            var prop = preprocessorType.GetProperty(nameof(State));
            var state = (State)prop.GetValue(null);
            state = state == State.Active ? 
                State.Inactive : State.Active;
            prop.SetValue(null, state);
        }
    }
}