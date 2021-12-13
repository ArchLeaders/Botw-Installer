using BotwInstaller.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BotwInstaller.Lib.Operations
{
    public static class Files
    {
        /// <summary>
        /// Enumerates all the safe file in a directory or drive.
        /// </summary>
        /// <param name="path">Target drive or directory</param>
        /// <param name="searchPattern">Search filter</param>
        /// <returns></returns>
        public static IEnumerable<string> GetSafeFiles(string path, string searchPattern = "*.*")
        {
            // Get all files in the root folder.
            foreach (string file in Directory.EnumerateFiles(path, searchPattern))
                yield return file;
            // Get all files in the sub folders
            foreach (string folder in Directory.EnumerateDirectories(path))
                if (!IsIgnorable(folder))
                    foreach (var file in Directory.EnumerateFiles(folder, searchPattern, SearchOption.AllDirectories))
                        yield return file;
        }

        /// <summary>
        /// Enumerates all the safe file in a directory or drive.
        /// </summary>
        /// <param name="path">Target drive or directory</param>
        /// <param name="searchPattern">Search filter</param>
        /// <returns></returns>
        public static List<string> GetSafeFilesNoYield(string path, string searchPattern = "*.*")
        {
            List<string> result = new List<string>();

            // Get all files in the root folder.
            foreach (string file in Directory.EnumerateFiles(path, searchPattern))
                result.Add(file);
            // Get all files in the sub folders
            foreach (string folder in Directory.EnumerateDirectories(path))
                if (!IsIgnorable(folder))
                {
                    try
                    {
                        foreach (var file in GetSafeFilesNoYield(folder, searchPattern))
                            result.Add(file);
                    }
                    catch { }
                }

            return result;
        }

        /// <summary>
        /// Tells GetSafeFiles if a folder is safe.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static bool IsIgnorable(string dir)
        {
            if (dir.ToLower().EndsWith("system volume information")) return true;
            if (dir.ToLower().EndsWith("documents and settings")) return true;
            if (dir.ToLower().StartsWith("c:\\$")) return true;
            if (dir.ToLower().StartsWith("c:\\onedrivetemp")) return true;
            if (dir.ToLower().StartsWith("c:\\program files")) return true;
            if (dir.ToLower().StartsWith("c:\\programdata")) return true;
            if (dir.ToLower().StartsWith("c:\\windows")) return true;
            if (dir.ToLower().Contains("$recycle.bin")) return true;
            return false;
        }
    }
}
