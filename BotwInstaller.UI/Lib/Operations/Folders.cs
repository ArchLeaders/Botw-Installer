#pragma warning disable CS8604

using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BotwInstaller.Lib.Operations
{
    internal class Folders
    {
        /// <summary>
        /// Copies the specified directory and all of it's contents to another directory.
        /// </summary>
        /// <param name="inputDir"></param>
        /// <param name="outputDir"></param>
        /// <param name="id"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
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

                        Prompt.Log($"[{id}] Copied {file} > {file.Replace(inputDir, outputDir)}");
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Prompt.Error("BotwInstallerLite.Lib.Operations.CopyAsync", new string[] { $"inputDir;{inputDir}", $"outputDir;{outputDir}", $"id;{id}", $"overwrite;{overwrite}" }, e.Message);
            }
        }
    }
}
