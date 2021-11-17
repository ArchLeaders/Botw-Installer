class GitHubReleaseInfo
{
    public Asset[]? assets { get; set; }

    public class Asset
    {
        public string? browser_download_url { get; set; }
    }

}