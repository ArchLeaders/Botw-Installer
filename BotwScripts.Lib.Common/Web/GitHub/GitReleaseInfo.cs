using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.Web.GitHub
{
    internal class GitReleaseInfo
    {
        public string url { get; set; } = "";
        public string assets_url { get; set; } = "";
        public string upload_url { get; set; } = "";
        public string html_url { get; set; } = "";
        public int id { get; set; } = -1;
        public Author author { get; set; } = new();
        public string node_id { get; set; } = "";
        public string tag_name { get; set; } = "";
        public string target_commitish { get; set; } = "";
        public string name { get; set; } = "";
        public bool draft { get; set; } = false;
        public bool prerelease { get; set; } = false;
        public DateTime created_at { get; set; } = new();
        public DateTime published_at { get; set; } = new();
        public Asset[] assets { get; set; } = new Asset[0];
        public string tarball_url { get; set; } = "";
        public string zipball_url { get; set; } = "";
        public string body { get; set; } = "";
    }

    public class Author
    {
        public string login { get; set; } = "";
        public int id { get; set; } = -1;
        public string node_id { get; set; } = "";
        public string avatar_url { get; set; } = "";
        public string gravatar_id { get; set; } = "";
        public string url { get; set; } = "";
        public string html_url { get; set; } = "";
        public string followers_url { get; set; } = "";
        public string following_url { get; set; } = "";
        public string gists_url { get; set; } = "";
        public string starred_url { get; set; } = "";
        public string subscriptions_url { get; set; } = "";
        public string organizations_url { get; set; } = "";
        public string repos_url { get; set; } = "";
        public string events_url { get; set; } = "";
        public string received_events_url { get; set; } = "";
        public string type { get; set; } = "";
        public bool site_admin { get; set; } = false;
    }

    public class Asset
    {
        public string url { get; set; } = "";
        public int id { get; set; } = -1;
        public string node_id { get; set; } = "";
        public string name { get; set; } = "";
        public object? label { get; set; } = null;
        public Uploader uploader { get; set; } = new();
        public string content_type { get; set; } = "";
        public string state { get; set; } = "";
        public int size { get; set; } = -1;
        public int download_count { get; set; } = -1;
        public DateTime created_at { get; set; } = new();
        public DateTime updated_at { get; set; } = new();
        public string browser_download_url { get; set; } = "";
    }

    public class Uploader
    {
        public string login { get; set; } = "";
        public int id { get; set; } = -1;
        public string node_id { get; set; } = "";
        public string avatar_url { get; set; } = "";
        public string gravatar_id { get; set; } = "";
        public string url { get; set; } = "";
        public string html_url { get; set; } = "";
        public string followers_url { get; set; } = "";
        public string following_url { get; set; } = "";
        public string gists_url { get; set; } = "";
        public string starred_url { get; set; } = "";
        public string subscriptions_url { get; set; } = "";
        public string organizations_url { get; set; } = "";
        public string repos_url { get; set; } = "";
        public string events_url { get; set; } = "";
        public string received_events_url { get; set; } = "";
        public string type { get; set; } = "";
        public bool site_admin { get; set; } = false;
    }
}
