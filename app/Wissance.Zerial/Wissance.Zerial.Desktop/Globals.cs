using Jeek.Avalonia.Localization;
using Wissance.Zerial.Desktop.Models;

namespace Wissance.Zerial.Desktop
{
    public static class Globals
    {
        static Globals()
        {
            CurrentAppVersion = new AppVersionModel()
            {
                Version = Localizer.Get(VersionKey),
                CompanyWebSite = Localizer.Get(CompanyWebsiteKey),
                Company = Localizer.Get(CompanyKey),
                Author = Localizer.Get(AuthorKey)
            };
        }

        // todo(UMV): place Links Here
        public static AppVersionModel CurrentAppVersion { get; }

        private const string VersionKey = "Zerial_Version";
        private const string CompanyKey = "Zerial_Owner";
        private const string AuthorKey = "Zerial_Authors";
        private const string CompanyWebsiteKey = "Zerial_Owner_Website";

        // public const string AppVersion = "1.0";
        public const string WissanceWebSiteUrl = "https://wissance.com/en"; //todo(use from model)
        public const string MVUGithubUrl = "https://github.com/EvilLord666";
        public const string SupportUrl = "https://github.com/Wissance/Zerial/blob/master/Support.md";
    }
}