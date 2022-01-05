using DarkNovelsDownloader.Commands;
using DarkNovelsDownloader.Common;
using DarkNovelsDownloader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace DarkNovelsDownloader.ViewModels
{
    class ViewModelMain : ViewModelBase
    {
        private readonly static string _appPath = Environment.CurrentDirectory;

        public List<Chapter> SelectedChapters { get; set; }

        private ObservableCollection<Chapter> _chaptersList { get; set; }

        public ObservableCollection<Chapter> ChaptersList
        {
            get => _chaptersList;
            set
            {
                _chaptersList = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GetChaptersCommand { get; }
        public RelayCommand GetTextCommand { get; }

        public ViewModelMain()
        {
            GetChaptersCommand = new RelayCommand(async (obj) => await GetChapters());
            GetTextCommand = new RelayCommand(async (obj) => await GetText());
        }

        private async Task GetText()
        {
            try
            {
                var chaptersIds = new List<string>();
                foreach(var c in SelectedChapters)
                {
                    chaptersIds.Add(c.Id);
                }
                var getChaptersSettings = new GetChaptersSettings
                {
                    ConvertImageSettings = new ConvertImageSettings { EngineFolderPath = _appPath, Lang = "rus" },
                    ExtractZipSettings = new ExtractZipSettings { ZipPath = _appPath + "\\data.bin", ExtractPath = _appPath + "\\Chapters" },
                    GetArchieveSettings = new GetArchieveSettings { B = Settings.Instance.BookId, C = chaptersIds, F = Settings.Instance.Format, Token = Settings.Instance.Token }
                };

                await Task.Factory.StartNew(() => { MainFunctions.GetChaptersText(getChaptersSettings, SelectedChapters); });
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetChapters()
        {
            try
            {
                var chapters = await MainFunctions.GetChapters(Settings.Instance.BookId, Settings.Instance.Token);

                ChaptersList = new ObservableCollection<Chapter>(chapters.Chapters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}