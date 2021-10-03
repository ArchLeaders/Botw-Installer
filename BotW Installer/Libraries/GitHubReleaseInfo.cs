using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    class GitHubReleaseInfo
    {
        public Asset[] assets { get; set; }

        public class Asset
        {
            public string browser_download_url { get; set; }
        }

    }
}
