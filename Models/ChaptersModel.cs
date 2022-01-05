using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace DarkNovelsDownloader.Models
{
    class ChaptersModel
    {
        [JsonProperty("data")]
        public ObservableCollection<Chapter> Chapters { get; set; }

        public static explicit operator ObservableCollection<object>(ChaptersModel v)
        {
            throw new NotImplementedException();
        }
    }

    class Chapter
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("visible", NullValueHandling = NullValueHandling.Ignore)]
        public long? Visible { get; set; }

        [JsonProperty("payed", NullValueHandling = NullValueHandling.Ignore)]
        public long? Payed { get; set; }

        [JsonProperty("purchase", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Purchase { get; set; }
    }
}
