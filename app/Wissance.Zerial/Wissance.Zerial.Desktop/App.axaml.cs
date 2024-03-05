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
            // appsettings is optional due to we don't actually know install dir i.e. in Snap, but we could execute from any working dir
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsFile, optional: true, reloadOnChange: true);

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