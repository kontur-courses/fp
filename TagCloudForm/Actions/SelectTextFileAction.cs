using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TagCloud.TextProvider;
using TagCloud.Visualization;
using TagCloudForm.Settings;

namespace TagCloudForm.Actions
{
    public class SelectTextFileAction : IUiAction
    {
        private readonly IDirectoryProvider directoryProvider;
        private readonly TxtFileReader txtFileReader;
        private readonly CloudVisualization cloudVisualization;
        private readonly CloudPainter cloudPainter;


        public SelectTextFileAction(IDirectoryProvider directoryProvider, TxtFileReader txtFileReader,
            CloudVisualization cloudVisualization, CloudPainter cloudPainter)
        {
            this.directoryProvider = directoryProvider;
            this.txtFileReader = txtFileReader;
            this.cloudVisualization = cloudVisualization;
            this.cloudPainter = cloudPainter;
        }

        public string Category => "Текст";
        public string Name => "Выбрать файл";
        public string Description => "Выбрать файл с текстом";

        public void Perform()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                InitialDirectory = Path.GetFullPath(directoryProvider.Directory),
                DefaultExt = "txt",
                Multiselect = true
            };
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                txtFileReader.FilesPaths = new HashSet<string>(dialog.FileNames);
                cloudVisualization.ResetWordsFrequenciesDictionary();
                cloudPainter.Paint();
            }
        }
    }
}