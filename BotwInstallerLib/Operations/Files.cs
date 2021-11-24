using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwInstallerLib.Operations
{
    public static class Files
    {
        public static IEnumerable<string> GetSafeFiles(string path, string searchPattern = "*.*")
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
        private static bool IsIgnorable(string dir)
        {
            if (dir.EndsWith("System Volume Information")) return true;
            if (dir.EndsWith("Documents and Settings")) return true;
            if (dir.Contains("$RECYCLE.BIN")) return true;
            return false;
        }
    }
}
