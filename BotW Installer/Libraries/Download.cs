using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Download
    {
        public static string Latest(string json)
        {
            var gitinfo = JsonConvert.DeserializeObject<GitHubReleaseInfo>(File.ReadAllText(json));
            var link = gitinfo.assets[0].browser_download_url;

            File.Delete(json);

            return link;
        }

        public static async Task GitInfo(string api, string json)
        {
            HttpClient client = new();

            client.DefaultRequestHeaders.Add("user-agent", "test");
            var _json = await client.GetStringAsync(api);
            await Task.Run(() => File.WriteAllText(json, _json));
        }

        public static async Task WebLink(string link, string outFile)
        {
            using (var client = new WebClient())
            {
                await Task.Run(() => client.DownloadFile(link, outFile));
            }
        }
    }
}
