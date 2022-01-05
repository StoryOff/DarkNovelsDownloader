using System.ComponentModel;
using System.Linq;
using DarkNovelsDownloader.Models;
using DarkNovelsDownloader.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace DarkNovelsDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelMain();
            if (string.IsNullOrEmpty(Settings.Instance.Token))
            {
                GetToken();
            }
        }

        public void MainWindowClosing(object sender, CancelEventArgs e)
        {
            Settings.Instance.Save();
            e.Cancel = false;
        }

        private async void GetToken()
        {
            var token = await this.ShowInputAsync("", "Insert your Token:");
            if (!string.IsNullOrEmpty(token) && token.Length > 5)
            {
                Settings.Instance.Token = token;
            }
            else MainWindowClosing(null, null);
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var viewmodel = (ViewModelMain)DataContext;
            viewmodel.SelectedChapters = listview.SelectedItems
                .Cast<Chapter>()
                .ToList();
        }
    }
}
