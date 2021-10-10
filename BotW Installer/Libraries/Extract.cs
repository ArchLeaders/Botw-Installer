using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

namespace BotW_Installer.Libraries
{
    public class Extract
    {
        public static async Task Zip(string zip, string outFolder)
        {
            await Task.Run(() => ZipFile.ExtractToDirectory(zip, outFolder));
        }

        public static async Task SevenZip(string zip, string outFolder)
        {
            if (!File.Exists($"{Data.temp}\\7z.resource"))
                await Task.Run(() => Embed("7z.resource", $"{Data.temp}\\7z.resource"));

            Process extract = new();
            extract.StartInfo.FileName = $"{Data.temp}\\7z.resource";
            extract.StartInfo.Arguments = $"x -y -o\"{outFolder}\" \"{zip}\"";
            extract.StartInfo.CreateNoWindow = true;

            extract.Start();
            await extract.WaitForExitAsync();
        }

        public static void Embed(string fileName, string output)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("BotW_Installer.Resource." + fileName))
            using (BinaryReader binaryReader = new(stream))
            using (FileStream fileStream = new(output, FileMode.OpenOrCreate))
            using (BinaryWriter binaryWriter = new(fileStream))
                binaryWriter.Write(binaryReader.ReadBytes((int)stream.Length));
        }
    }
}
