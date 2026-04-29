using System;
using System.IO;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Jeek.Avalonia.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wissance.Zerial.Desktop.Views;

namespace Wissance.Zerial.Desktop
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDir = Path.GetDirectoryName(assemblyLocation);
            string localizationDir = Path.Combine(assemblyDir, LocalizationJsonDir);
            Localizer.SetLocalizer(new JsonLocalizer(localizationDir));
            SetDefaultLanguage();
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new SplashScreenWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void SetDefaultLanguage()
        {
            string countryCode = DefaultCountryCode;
            try
            {
                string currentCulture = CultureInfo.CurrentUICulture.Name;
                string[] parts = currentCulture.Split("-");
                if (Localizer.Languages.Contains(parts[0].ToLower()))
                {
                    countryCode = parts[0].ToLower();
                }
            }
            catch (Exception e) {  }

            Localizer.Language = countryCode;
        }

        private const string DefaultCountryCode = "en";
        private const string LocalizationJsonDir = "Assets/Languages";
    }
}