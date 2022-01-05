using DarkNovelsDownloader.Common;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace DarkNovelsDownloader.ViewModels
{
    class Settings : JsonSettings
    {
        private static Settings _instance;
        public static Settings Instance
        {
            get => _instance ??= new Settings();
        }

        private string _token;
        private string _bookId;
        private string _format;
        private ObservableCollection<string> _formatsList = new ObservableCollection<string> { "docx", "png", "pdf", "fb2", "epub" };
        [JsonIgnore]
        public ObservableCollection<string> FormatsList
        {
            get => _formatsList;
            set
            {
                _formatsList = value;
                RaisePropertyChanged();
            }
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string BookId
        {
            get => _bookId;
            set => SetProperty(ref _bookId, value);
        }

        public string Format
        {
            get => _format;
            set => SetProperty(ref _format, value);
        }
    }
}