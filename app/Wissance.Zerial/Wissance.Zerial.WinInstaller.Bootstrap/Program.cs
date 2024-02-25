using System.Diagnostics;

namespace Wissance.Zerial.WinInstaller.Bootstrap
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // 1. Download .exe installer depends on OS/CPU architecture
            string installerUrl = Environment.Is64BitOperatingSystem ? WinX64Installer : WinX86Installer;
            string fileName = "Wissance.Zerial.Installer.exe";
            string fullFilePath = Path.Combine(Path.GetFullPath("."), fileName);
            Console.WriteLine("******** 1. Prepare to Get Installer ********");
            using (HttpClient client = new HttpClient())
            {
                await client.DownloadFileTaskAsync(new Uri(installerUrl), fullFilePath);
            }
            
            Console.WriteLine("******** 2. Run installer Silently ********");
            // 2. Run installer silently
            Process installerProcess = Process.Start(fullFilePath, "/SILENT");
            installerProcess.Close();
            installerProcess.Dispose();
            
        }

        private const string WinX64Installer = "https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.Win.X64.exe";
        private const string WinX86Installer = "https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.Win.X86.exe";
    }
}