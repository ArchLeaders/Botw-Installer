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
            await Task.Run(() => ZipFile.ExtractToDirectory(zip, outFolder, true));
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
