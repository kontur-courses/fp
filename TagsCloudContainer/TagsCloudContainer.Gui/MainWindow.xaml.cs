using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CSharpFunctionalExtensions;
using TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.Gui;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IImageListProvider, ISettingsFactory
{
    private readonly Func<ISettingsCreator<CircularLayouterAlgorithmSettings>>
        circularLayouterSettingsEditorFactory;

    private readonly Func<ISettingsCreator<ClassicDrawerSettings>> classicDrawerSettingsEditorFactory;

    private readonly IFunnyWordsSelector funnyWordsSelector;

    private readonly Func<ISettingsEditor<GuiGraphicsProviderSettings>> guiGraphicsProviderSettingsEditorFactory;

    private readonly Func<MultiDrawer> multiDrawerFactory;

    private readonly Func<ISettingsCreator<RandomColoredDrawerSettings>> randomColoredDrawerSettingsCreator;

    private readonly Timer timer;

    public MainWindow(
        Func<ISettingsCreator<CircularLayouterAlgorithmSettings>> circularLayouterSettingsEditorFactory,
        Func<ISettingsCreator<ClassicDrawerSettings>> classicDrawerSettingsEditorFactory,
        Func<ISettingsEditor<GuiGraphicsProviderSettings>> guiGraphicsProviderSettingsEditorFactory,
        Func<ISettingsCreator<RandomColoredDrawerSettings>> randomColoredDrawerSettingsCreator,
        Func<MultiDrawer> multiDrawerFactory,
        IFunnyWordsSelector funnyWordsSelector)
    {
        this.circularLayouterSettingsEditorFactory = circularLayouterSettingsEditorFactory;
        this.classicDrawerSettingsEditorFactory = classicDrawerSettingsEditorFactory;
        this.guiGraphicsProviderSettingsEditorFactory = guiGraphicsProviderSettingsEditorFactory;
        this.randomColoredDrawerSettingsCreator = randomColoredDrawerSettingsCreator;
        this.multiDrawerFactory = multiDrawerFactory;
        this.funnyWordsSelector = funnyWordsSelector;
        InitializeComponent();
        DataContext = this;
        LayouterAlgorithmSettingsList.CollectionChanged += (_, _) => StartingThrottlingOnWork();
        DrawerSettingsList.CollectionChanged += (_, _) => StartingThrottlingOnWork();
        timer = new(StartDrawingByTimer);
    }

    public ObservableCollection<DrawerSettings> DrawerSettingsList { get; } = new()
        { new ClassicDrawerSettings() };

    public ObservableCollection<LayouterAlgorithmSettings> LayouterAlgorithmSettingsList { get; } = new()
        { new CircularLayouterAlgorithmSettings() };

    private GuiGraphicsProviderSettings GraphicsSettings { get; set; } = new();

    public ObservableCollection<byte[]> ImageBytes { get; set; } = new();

    private bool AutoDraw { get; set; }

    public LayouterAlgorithmSettings? SelectedLayouterAlgorithmSettings { get; set; }

    public DrawerSettings? SelectedDrawerSettings { get; set; }

    public Result AddImageBits(byte[] imageBytes)
    {
        ImageBytes.Add(imageBytes);
        return Result.Success();
    }

    public Result<Settings> Build()
    {
        return Result.Success(new Settings
        {
            GraphicsProviderSettings = GraphicsSettings,
            LayouterAlgorithmSettings = LayouterAlgorithmSettingsList.ToList(),
            DrawerSettings = DrawerSettingsList.ToList()
        });
    }

    private void StartingThrottlingOnWork()
    {
        if (!AutoDraw) return;
        timer.Change(TimeSpan.FromSeconds(2), Timeout.InfiniteTimeSpan);
    }

    private void WordsChanged(object sender, RoutedEventArgs e)
    {
        StartingThrottlingOnWork();
    }

    private void RemoveSelectedDrawerSettings(object sender, RoutedEventArgs e)
    {
        if (SelectedDrawerSettings is not null)
            DrawerSettingsList.Remove(SelectedDrawerSettings);
        StartingThrottlingOnWork();
    }

    private void NewClassicDrawerSettings(object sender, RoutedEventArgs e)
    {
        var editor = classicDrawerSettingsEditorFactory();
        CallCreator(editor, DrawerSettingsList);
    }

    private void RemoveSelectedLayouterAlgorithmSettings(object sender, RoutedEventArgs e)
    {
        if (SelectedLayouterAlgorithmSettings is not null)
            LayouterAlgorithmSettingsList.Remove(SelectedLayouterAlgorithmSettings);
        StartingThrottlingOnWork();
    }

    private void NewCircularLayouterAlgorithmSettings(object sender, RoutedEventArgs e)
    {
        var editor = circularLayouterSettingsEditorFactory();
        CallCreator(editor, LayouterAlgorithmSettingsList);
    }

    private void AutoDrawIsChanged(object sender, RoutedEventArgs e)
    {
        AutoDraw = ((CheckBox)sender).IsChecked ?? false;
    }

    private void StartDrawing(object sender, RoutedEventArgs e)
    {
        timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        _ = StartDrawingAsync();
    }

    private void StartDrawingByTimer(object? state)
    {
        _ = StartDrawingAsync();
    }

    private async Task StartDrawingAsync()
    {
        Dispatcher.Invoke(() =>
        {
            ImageBytes.Clear();
            ProgressBar.Visibility = Visibility.Visible;
            ControlPanel.Visibility = Visibility.Collapsed;
        }, DispatcherPriority.Background);

        await Dispatcher.InvokeAsync(() =>
            {
                var allWords = Words.Text.ReplaceLineEndings("\n").Split('\n',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = Result.Success((drawer: multiDrawerFactory(), selector: funnyWordsSelector))
                    .Bind(
                        tuple => Result.Success((tuple.drawer, tuple.selector, allWords)))
                    .Bind(tuple => (tuple.drawer, wordsResult: tuple.selector.RecognizeFunnyCloudWords(tuple.allWords)))
                    .Ensure(tuple => tuple.wordsResult.IsSuccess, err => err.wordsResult.Error)
                    .Bind(tuple => (tuple.drawer, words: tuple.wordsResult.Value))
                    .Bind(tuple => tuple.drawer.Draw(tuple.words));
                if (result.IsFailure)
                    MessageBox.Show(this, result.Error, "Failed to draw clouds", MessageBoxButton.OK,
                        MessageBoxImage.Error);
            }, DispatcherPriority.Background).Task
            .ContinueWith(_ => Dispatcher.Invoke(() =>
            {
                ControlPanel.Visibility = Visibility.Visible;
                ProgressBar.Visibility = Visibility.Collapsed;
            }));
    }

    private void EditGraphicsProviderSettings(object sender, RoutedEventArgs e)
    {
        var editor = guiGraphicsProviderSettingsEditorFactory();
        Hide();
        GraphicsSettings = editor.ShowEdit(GraphicsSettings);
        Show();
        StartingThrottlingOnWork();
    }

    private void NewRandomColoredDrawerSettings(object sender, RoutedEventArgs e)
    {
        var editor = randomColoredDrawerSettingsCreator();
        CallCreator(editor, DrawerSettingsList);
    }

    private void CallCreator<T, TBase>(ISettingsCreator<T> editor, ICollection<TBase> collection) where T : TBase
    {
        Hide();
        editor.ShowCreate()
            .Tap(x => collection.Add(x))
            .Tap(StartingThrottlingOnWork)
            .Anyway(_ => Show());
    }
}