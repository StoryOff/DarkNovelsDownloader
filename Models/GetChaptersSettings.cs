using System;
using System.Collections.Generic;
using System.Text;

namespace DarkNovelsDownloader.Models
{
    class GetChaptersSettings
    {
        public GetArchieveSettings GetArchieveSettings { get; set; }
        public ConvertImageSettings ConvertImageSettings { get; set; }
        public ExtractZipSettings ExtractZipSettings { get; set; }
    }
}
