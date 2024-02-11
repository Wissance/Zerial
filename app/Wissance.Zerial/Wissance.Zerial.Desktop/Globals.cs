using Wissance.Zerial.Desktop.Models;

namespace Wissance.Zerial.Desktop
{
    public static class Globals
    {
        // todo(UMV): place Links Here
        public static AppVersionModel CurrentAppVersion
        {
            get
            {
                return _appVersion;
            }
        }

        private static readonly AppVersionModel _appVersion = new AppVersionModel()
        {
            Version = AppVersion,
            CompanyWebSite = WissanceWebSiteUrl,
            Company = "Wissance LLC (ООО \"Висанс\")",
            Author = "M.V.Ushakov"
        };
        
        public const string ConnectButtonConnectText = "Connect";
        public const string ConnectButtonDisconnectText = "Disconnect";

        public const string AppVersion = "1.0";
        public const string WissanceWebSiteUrl = "https://wissance.com/en"; //todo(use from model)
        public const string MVUGithubUrl = "https://github.com/EvilLord666";
        public const string SupportUrl = "https://github.com/Wissance/Zerial/blob/master/Support.md";
    }
}