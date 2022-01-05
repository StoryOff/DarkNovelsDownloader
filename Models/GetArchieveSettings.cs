using System.Collections.Generic;

namespace DarkNovelsDownloader.Models
{
    class GetArchieveSettings
    {
        public string B { get; set; }
        public string F { get; set; }
        public List<string> C { get; set; }
        public string Token { get; set; } = null;
    }
}
