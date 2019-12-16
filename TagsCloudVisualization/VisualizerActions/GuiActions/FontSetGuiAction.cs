using System.Drawing;
using System.Windows.Forms;
using TagsCloudVisualization.GUI;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.VisualizerActions.GuiActions
{
    public class FontSetGuiAction : FontSetAction, IGuiAction
    {
        public FontSetGuiAction(AppSettings appSettings) : base(appSettings)
        {}

        public override string GetActionDescription()
        {
            return "Поменять шрифт";
        }

        public override string GetActionName()
        {
            return "Шрифт...";
        }

        protected override Font GetFont()
        {
            var fontDialog = new FontDialog {Font = appSettings.Font,
                MinSize = 1,
                MaxSize = 1};
            var dialogResult = fontDialog.ShowDialog();
            return dialogResult == DialogResult.OK 
                ? fontDialog.Font 
                : appSettings.Font;
        }

        public MenuCategory GetMenuCategory()
        {
            return MenuCategory.Settings;
        }
    }
}