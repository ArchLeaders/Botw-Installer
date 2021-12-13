#pragma warning disable CS8602

using System.Text.Json;

namespace BotwScripts.Lib.Common.Web.GitHub
{
    public static class GitHub
    {
        public static async Task<string> GetLatestRelease(this string api, int index = 1)
        {
            // Decode simplified form (User;Repo)
            if (!api.Contains("api.github.com"))
            {
                var info = api.Split(';');
                api = $"https://api.github.com/repos/{info[0]}/{info[1]}/releases/latest";
            }

            // Create HttpsClient
            using (HttpClient client = new())
            {
                // Setup request headers
                client.DefaultRequestHeaders.Add("user-agent", "test");

                // Get API information
                string _json = await client.GetStringAsync(api);

                // Parse API information
                GitReleaseInfo? gitinfo = JsonSerializer.Deserialize<GitReleaseInfo>(_json);

                // Return the desired asset download link
                return gitinfo.assets[index - 1].browser_download_url;
            }
        }

        public static async Task<string> GetLatestPreRelease(this string api, int index = 1)
        {
            // Decode simplified form (User;Repo)
            if (!api.Contains("api.github.com"))
            {
                var info = api.Split(';');
                api = $"https://api.github.com/repos/{info[0]}/{info[1]}/releases";
            }

            using (HttpClient client = new())
            {
                // Setup request headers
                client.DefaultRequestHeaders.Add("user-agent", "test");

                // Get API information
                string _json = await client.GetStringAsync(api);

                // Parse API information to a list
                List<GitReleaseInfo>? gitinfo = JsonSerializer.Deserialize<List<GitReleaseInfo>>(_json);

                // Return the desired asset download link
                return gitinfo[0].assets[index - 1].browser_download_url;
            }
        }
    }
}
