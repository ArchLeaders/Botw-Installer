#pragma warning disable CS8604

using BotwScripts.Lib.Common;

namespace BotwScripts.Lib.Common.IO
{
    public static class Batch
    {
        /// <summary>
        /// Copies the specified directory and all of it's contents to another directory.
        /// </summary>
        /// <param name="inputDir"></param>
        /// <param name="outputDir"></param>
        /// <param name="id"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static async Task CopyDirAsync(Interface.Update update, Interface.Notify notify, int estimate, int count, string logFile, string inputDir, string outputDir, string id = "Unspecified", bool overwrite = true)
        {
            List<Task> tasks = new();
            List<string> log = new();

            notify($"Copying {count} files . . .");

            await Task.Run(async () =>
            {
                foreach (var file in Directory.EnumerateFiles(inputDir, "*.*", SearchOption.AllDirectories))
                {
                    // Collect the DirectoryInfo & FileInfo
                    FileInfo fileInfo = new(file);
                    DirectoryInfo directoryInfo = new(fileInfo.DirectoryName);

                    // Log the copy operation
                    log.Add($"");

                    // Create the output directory
                    await Task.Run(() => Directory.CreateDirectory(directoryInfo.FullName.Replace(inputDir, outputDir)));

                    // Copy the file
                    tasks.Add(Task.Run(() => File.Copy(file, file.Replace(inputDir, outputDir), overwrite)));
                }
            });

            await Task.WhenAll(tasks);

            await Interface.Log($"[{id}] [{DateTime.Now}] Copied {log.Count} files from '{inputDir}' to '{outputDir}'", logFile);
            notify($"Copied {log.Count} files.");
        }
    }
}
