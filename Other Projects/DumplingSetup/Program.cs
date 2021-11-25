#pragma warning disable CS8600
#pragma warning disable CS8602

using Newtonsoft.Json;
using System.IO.Compression;

Console.Write("Enter your SD card drive letter: ");
string sdLetter = Console.ReadLine().ToUpper();

if (sdLetter.Contains(":\\"))
    sdLetter = $"{sdLetter}";
else if (sdLetter.Contains(":"))
    sdLetter = $"{sdLetter}\\";
else
    sdLetter = $"{sdLetter}:\\";

while (sdLetter == "" | !Directory.Exists(sdLetter))
{
    Console.Write("Enter your SD card drive letter: ");
    sdLetter = Console.ReadLine();
}

foreach (string file in Directory.GetDirectories(sdLetter))
    if (file != null)
    {
        Console.WriteLine($"SD Card not empty. Please select an empty drive or format the SD card as Fat32 and retry. (Make sure to backup any wanted files before formating)");
        Console.ReadLine();
        return;
    }

try
{
    List<Task> download = new();

    download.Add(DownloadFile("https://wiiubru.com/appstore/zips/wup_installer_gx2.zip", "./wup_installer.zip"));
    download.Add(DownloadFile("https://www.wiiubru.com/appstore/zips/nanddumper.zip", "./nanddumper.zip"));
    download.Add(GitHubLatest("https://api.github.com/repos/vgmoose/hbas/releases/latest", "./appstore.zip", 4));
    download.Add(DownloadFile("https://github.com/dimok789/homebrew_launcher/releases/download/1.4/homebrew_launcher.v1.4.zip", "./homebrew.zip"));
    download.Add(DownloadFile("https://www.wiiubru.com/appstore/zips/mocha.zip", "./mocha.zip"));
    download.Add(DownloadFile("https://wiiu.hacks.guide/docs/files/SaveMii_Mod.zip", "./savemii.zip"));
    download.Add(GitHubLatest("https://api.github.com/repos/emiyl/dumpling/releases/latest", $"./dumpling.zip"));
    download.Add(DownloadFile("https://wiiu.hacks.guide/docs/files/config.ini", "./config.ini"));
    download.Add(GitHubLatest("https://api.github.com/repos/wiiu-env/homebrew_launcher_installer/releases/latest", "./payload.zip"));

    await Task.WhenAll(download);

    await Extract("wup_installer", sdLetter);
    await Extract("nanddumper", sdLetter);
    await Extract("appstore", sdLetter);
    await Extract("homebrew", sdLetter);
    await Extract("mocha", sdLetter);
    await Extract("savemii", sdLetter);
    await Extract("dumpling", sdLetter);
    await Extract("payload", sdLetter);

    await Task.Run(() => File.Copy("./config.ini", $"{sdLetter}\\wiiu\\apps\\mocha\\config.ini"));
    await Task.Run(() => File.Delete("./config.ini"));

    Console.WriteLine($"\nCompleted!\nHomebrew is installed on SD Card {sdLetter}");
    Console.ReadLine();
}
catch (Exception e)
{
    CWrite(e.Message, ConsoleColor.DarkRed);
}

static async Task GitHubLatest(string api, string outFile, int asset = 1)
{
    HttpClient client = new();

    CWrite($"Requesting {api}...", ConsoleColor.DarkCyan);
    client.DefaultRequestHeaders.Add("user-agent", "test");
    var _json = await client.GetStringAsync(api);
    var gitinfo = JsonConvert.DeserializeObject<GitHubReleaseInfo>(_json);
    var link = gitinfo.assets[asset - 1].browser_download_url;

    CWrite($"Downloading {link}...");
    byte[] file = await client.GetByteArrayAsync(link);

    CWrite($"Writting {outFile}", ConsoleColor.DarkGray);
    File.WriteAllBytes(outFile, file);
}

static async Task DownloadFile(string link, string outFile)
{
    HttpClient client = new();

    CWrite($"Downloading {link}...");
    var file = await client.GetByteArrayAsync(link);

    CWrite($"Writting {outFile}", ConsoleColor.DarkGray);
    await Task.Run(() => File.WriteAllBytes(outFile, file));
}

static async Task Extract(string zipFile, string sd)
{
    CWrite($"Extracting ./{zipFile}.zip...", ConsoleColor.DarkYellow);
    await Task.Run(() => ZipFile.ExtractToDirectory($"./{zipFile}.zip", sd, true));
    await Task.Run(() => File.Delete($"./{zipFile}.zip"));
}

static void CWrite(string msg, ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.WriteLine(msg);
    Console.ResetColor();
}