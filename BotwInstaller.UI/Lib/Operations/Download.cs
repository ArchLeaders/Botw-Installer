#pragma warning disable CS8602

using BotwInstaller.Lib.Exceptions;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations
{
    public static class Download
    {
        /// <summary>
        /// Attempts to download a file from the passed URL.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static async Task FromUrl(string url, string output)
        {
            try
            {
                Prompt.Log($"Downloading {url}...");
                using (var client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(output, bytes);
                }
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Download.FromUrl", new string[] { $"url;{url}", $"output;{output}" }, e.Message);
            }
        }

        /// <summary>
        /// Get the latest release URL from a GitHub api.
        /// </summary>
        /// <param name="api">GitHub latest release api.</param>
        /// <param name="index">Asset index.</param>
        /// <returns></returns>
        public static async Task<string> GetRelease(this string api, int index = 1)
        {
            try
            {
                if (!api.Contains("api.github.com")) // If true, assume form 'User;Repo'
                {
                    var info = api.Split(';');
                    api = $"https://api.github.com/repos/{info[0]}/{info[1]}/releases/latest";
                }

                HttpClient client = new();

                Prompt.Log($"Requesting {api}...");
                client.DefaultRequestHeaders.Add("user-agent", "test");
                var _json = await client.GetStringAsync(api);
                var gitinfo = JsonSerializer.Deserialize<GitHubReleaseInfo>(_json);
                return gitinfo.assets[index - 1].browser_download_url;
            }
            catch (Exception ex)
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Download.ToUrl", new string[] { $"this api;{api}", $"index;{index}" }, ex.Message);
                return "";
            }
        }
    }
}
