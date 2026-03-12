using System.Diagnostics;
using System.Formats.Tar;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace ExcelShSy.Setup
{
    public static class Updater
    {
        // args[0] - path to archive
        // args[1] - target directory
        // args[2] - name executable file to start after unpacking args[1]+args[2] = path to exe
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: Updater <archive> <targetDir> <executableFileName>");
                return;
            }
            var finalDir = args[1];
            Thread.Sleep(2000);
            try
            {
                if (!Directory.Exists(finalDir))
                    Directory.CreateDirectory(finalDir);
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    UnpackZip(args[0], finalDir);
                else
                    UnpackTarGz(args[0], finalDir);
            }
            finally
            {
                File.Delete(args[0]);
                
                var psi = new ProcessStartInfo
                {
                    FileName = $"{Path.Combine(args[1], args[2])}",
                    UseShellExecute = true
                };

                Process.Start(psi);
                Environment.Exit(0);
            }
        }
        
        private static void UnpackZip(string archive, string targetDir)
        {
            ZipFile.ExtractToDirectory(archive, targetDir, true);
        }
        
        private static void UnpackTarGz(string archive, string targetDir)
        {
            using var fs = File.OpenRead(archive);
            using var gzip = new GZipStream(fs, CompressionMode.Decompress);
            using var tar = new TarReader(gzip);
            while (tar.GetNextEntry() is { } entry)
            {
                var fullPath = Path.Combine(targetDir, entry.Name);
                if (entry.EntryType == TarEntryType.Directory)
                    Directory.CreateDirectory(fullPath);
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                    using var outStream = File.Create(fullPath);
                    entry.DataStream!.CopyTo(outStream);
                }
            }
        }
    }
}

