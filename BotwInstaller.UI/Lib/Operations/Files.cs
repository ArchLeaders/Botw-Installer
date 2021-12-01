using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BotwInstaller.Lib.Operations
{
    public static class Files
    {
        /// <summary>
        /// Gets all the safe file in a directory or drive.
        /// </summary>
        /// <param name="path">Target drive or directory</param>
        /// <param name="searchPattern">Search filter</param>
        /// <returns></returns>
        public static IEnumerable<string> GetSafeFiles(string path, string searchPattern = "*.*")
        {
            try
            {
                // Get all files in the root folder.
                foreach (string file in Directory.EnumerateFiles(path, searchPattern))
                    yield return file;
                // Get all files in the sub folders
                foreach (string folder in Directory.EnumerateDirectories(path))
                    if (!IsIgnorable(folder))
                    {
                        try
                        {
                            foreach (var file in Directory.EnumerateFiles(folder, searchPattern, SearchOption.AllDirectories))
                                yield return file;
                        }
                        finally { }
                    }
            }
            finally
            {
                Prompt.Error("BotwInstaller.Lib.Operations.Files.GetSafeFile", new string[] { $"path;{path}", $"searchPattern;{searchPattern}" });
            }
        }

        /// <summary>
        /// Tells GetSafeFiles if a file or folder is safe.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static bool IsIgnorable(string dir)
        {
            if (dir.ToLower().EndsWith("system volume information")) return true;
            if (dir.ToLower().EndsWith("documents and settings")) return true;
            if (dir.ToLower().Contains("$recycle.bin")) return true;
            return false;
        }
    }
}
