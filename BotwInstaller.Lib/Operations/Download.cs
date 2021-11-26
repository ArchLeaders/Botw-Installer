using BotwInstaller.Lib.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations
{
    public static class Download
    {
        public static async Task FromUrl(string url, string output)
        {
            try
            {
                ConsoleMsg.PrintLine($"Downloading {url}...", ConsoleColor.DarkCyan);
                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(output, bytes);
                }
            }
            catch (Exception e)
            {
                ConsoleMsg.Error("BotwInstaller.Lib.Operations.Download.FromUrl", new string[] { $"url;{url}", $"output;{output}" }, e.Message);
            }
        }

        public static async Task<string> ToUrl(this string api, int index = 1)
        {
            if (!api.Contains("api.github.com")) // If true, assume form 'User;Repo'
            {
                var info = api.Split(';');
                api = $"https://api.github.com/repos/{info[0]}/{info[1]}/releases/latest";
            }

            HttpClient client = new();

            ConsoleMsg.PrintLine($"Requesting {api}...", ConsoleColor.Cyan);
            client.DefaultRequestHeaders.Add("user-agent", "test");
            var _json = await client.GetStringAsync(api);
            var gitinfo = JsonSerializer.Deserialize<GitHubReleaseInfo>(_json);
            return gitinfo.assets[index - 1].browser_download_url;
        }
    }
}
