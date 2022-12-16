using Autofac;
using GuiApp.Controls;
using TagCloud;

namespace GuiApp;

internal static class Program
{
    private static IContainer container;

    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        container = DiContainerBuilder.Build();
        var form = container.Resolve<Form>();
        RenderButton.RenderRequired += OnRender;
        Application.Run(form);
        RenderButton.RenderRequired -= OnRender;
    }

    private static void OnRender(object? sender, EventArgs e)
    {
        var oldImage = Viewport.Instance.Image;
        var result = container.Resolve<TagCloudConstructor>().Construct();
        if (!result.IsSuccess)
            MessageBox.Show(result.Error, "Error", MessageBoxButtons.OK);
        Viewport.Instance.Image = result.Value;
        oldImage?.Dispose();
    }
}