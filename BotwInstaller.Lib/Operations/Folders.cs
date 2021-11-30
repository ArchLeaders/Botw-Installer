#pragma warning disable CS8604

using BotwInstaller.Lib.Prompts;

namespace BotwInstaller.Lib.Operations
{
    internal class Folders
    {
        public static async Task CopyAsync(string inputDir, string outputDir, string id = "Unspecified", bool overwrite = true)
        {
            try
            {
                List<Task> tasks = new();

                await Task.Run(async() =>
                {
                    foreach (var file in Directory.EnumerateFiles(inputDir, "*.*", SearchOption.AllDirectories))
                    {
                        FileInfo fileinfo = new(file);
                        DirectoryInfo directoryInfo = new(fileinfo.DirectoryName); /// CS8604

                        await Task.Run(() => Directory.CreateDirectory(directoryInfo.FullName.Replace(inputDir, outputDir)));
                        tasks.Add(Task.Run(() => File.Copy(file, file.Replace(inputDir, outputDir), overwrite)));

                        ConsoleMsg.Print($"{id} :: Copied {file} to {file.Replace(inputDir, outputDir)}", ConsoleColor.DarkCyan, true, false);
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                ConsoleMsg.Error("BotwInstallerLite.Lib.Operations.CopyAsync", new string[] { $"inputDir;{inputDir}", $"outputDir;{outputDir}", $"id;{id}", $"overwrite;{overwrite.ToString()}" }, e.Message);
            }
        }
    }
}
