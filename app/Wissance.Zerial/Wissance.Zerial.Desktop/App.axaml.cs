using System;
using System.IO;
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
            Localizer.SetLocalizer(new JsonLocalizer(LocalizationJsonDir));
            SetDefaultLanguage();
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new SplashScreenWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void SetDefaultLanguage()
        {
            Localizer.Language = "en";
        }

        private const string LocalizationJsonDir = "./Assets/Languages";
    }
}