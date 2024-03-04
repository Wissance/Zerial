using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wissance.Zerial.Desktop.Views;

namespace Wissance.Zerial.Desktop
{
    public class App : Application
    {
        static App()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsFile, optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            ServiceCollection services = new ServiceCollection();
            services.AddLogging();

            ServiceProvider = services.BuildServiceProvider();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new SplashScreenWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private const string AppSettingsFile = "appsettings.json";
    }
}