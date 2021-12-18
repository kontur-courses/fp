using System;
using System.Drawing;
using TagsCloudContainer.Common.Result;

namespace TagsCloudContainer.UI
{
    public class SetVisualizationSettingsAction : UiAction
    {
        public override string Category => "Visualization";
        public override string Name => "SetVisualizationSettings";
        public override string Description => "Change visualizator settings";

        public SetVisualizationSettingsAction(IResultHandler handler)
            : base(handler)
        {
        }

        protected override void PerformAction()
        {
            Result.OfAction(SetSize)
                .Then((n) => ShouldContinue(SetBackground))
                .Then((n) => ShouldContinue(SetFontFamily))
                .Then((n) => ShouldContinue(SetMinMargin))
                .Then((n) => ShouldContinue(SetFillingTags));
        }

        private void SetSize()
        {
            var size = new Size();
            void SetWidth()
            {
                var width = handler.GetText();
                if (!int.TryParse(width, out var w)) 
                    throw new Exception("Width should be int!");
                size.Width = w;
            }
            void SetHeight()
            {
                var height = handler.GetText();
                if (!int.TryParse(height, out var h))
                    throw new Exception("Height should be int!");
                size.Height = h;
            }

            handler.AddHandledText("Set Width of result image");
            handler.Handle(SetWidth);

            handler.AddHandledText("Set Height of result image");
            handler.Handle(SetHeight);
            
            AppSettings.ImageSize = size;
        }

        private void SetBackground()
        {
            void SetValue()
            {
                var clr = handler.GetText();
                AppSettings.BackgroundColor = Color.FromName(clr);
            }
            handler.AddHandledText("Set Color image background");
            handler.Handle(SetValue, "It is not a color, try set 'Red' color");
        }

        private void SetFontFamily()
        {
            void SetValue()
            {
                var family = handler.GetText();
                AppSettings.FontFamily = new FontFamily(family);
            }
            handler.AddHandledText("Set FontFamily of Tags");
            handler.Handle(SetValue, "It is not a FontFamily, try set 'Arial' family");
        }

        private void SetMinMargin()
        {
            void SetValue()
            {
                var minMargin = handler.GetText();
                if (!float.TryParse(minMargin, out var margin))
                    throw new Exception("Margin should be float!");
                AppSettings.MinMargin = margin;
            }
            handler.AddHandledText("Set minimal margin of cloud from borders");
            handler.Handle(SetValue);
        }

        private void SetFillingTags()
        {
            void SetValue()
            {
                var answer = handler.GetText();
                switch (answer)
                {
                    case "y":
                        AppSettings.FillTags = true;
                        return;
                    case "n":
                        AppSettings.FillTags = false;
                        return;
                    default:
                        throw new Exception("Answer should be 'y' or 'n'");
                }
            }
            handler.AddHandledText("Set Filling Tag rectangles by colors, yes or no? 'y', 'n'");
            handler.Handle(SetValue);
        }

        private void ShouldContinue(Action action)
        {
            handler.AddHandledText("Should continue change settings, yes or no? 'y', 'n'");
            while (true)
            {
                var answer = handler.GetText();
                switch (answer)
                {
                    case "y":
                        action();
                        return;
                    case "n":
                        throw new Exception("Changing visualizator settings ended.\n" +
                                            "If You want to change it again, you Should choose this action again");
                    default:
                        handler.AddHandledText("Answer should be 'y' or 'n'");
                        break;
                }
            }
        }
    }
}