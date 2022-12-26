using System;
using System.Windows.Forms;
using Autofac;
using TagsCloudContainer;

namespace TagsCloudGUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var bulder = new DIBuilder();
            var container = Result.Of(() => bulder.Build());
            if (container.IsSuccess)
                Application.Run(container.Value.Resolve<Form>());
        }
    }
}