using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotwScripts.Lib.Common.Web
{
    public static class Download
    {
        /// <summary>
        /// Collect file information from a Url and writes the <paramref name="output"/> file.
        /// </summary>
        /// <param name="url">Direct download link.</param>
        /// <param name="output">Output file.</param>
        /// <param name="timout">TimeSpan in seconds for the application to wait with no responce before failing.</param>
        /// <returns></returns>
        public static async Task FromUrl(string url, string output, double timout = 150)
        {
            using (HttpClient client = new())
            {
                client.Timeout = TimeSpan.FromSeconds(timout);
                var bytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(output, bytes);
            }
        }
    }
}
