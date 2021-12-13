namespace BotwInstaller.Lib
{
    class GitHubReleaseInfo
    {
        public Asset[] assets { get; set; } = new Asset[0];

        public class Asset
        {
            public string browser_download_url { get; set; } = "";
        }

    }
}