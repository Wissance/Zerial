using System.Collections.ObjectModel;
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

            Process p = new Process();
            p.StartInfo.FileName = $"{fullFilePath} ";
            p.StartInfo.Arguments = @"/VERYSILENT /SP- /SUPPRESSMSGBOXES";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.Verb = "runas";
            // p.StartInfo.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            p.Start();
            p.WaitForExit();
        }

        private const string WinX64Installer = "https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.Win.X64.exe";
        private const string WinX86Installer = "https://github.com/Wissance/Zerial/raw/master/app/Wissance.Zerial/Wissance.Zerial.Installer/Windows/Wissance.Zerial.Win.X86.exe";
    }
}