using Avalonia;
using Avalonia.ReactiveUI;
using NetEnvExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Wissance.Zerial.Desktop.Extensions;
using Wissance.Zerial.Desktop.Logging;

namespace Wissance.Zerial.Desktop
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // we are passing environment via --environment={env} snap or win-native
            Services = new ServiceCollection();
            string envKey = args.FirstOrDefault(a => a.Contains(EnvironmentKey));
            Environment = DefaultEnvironment;
            if (envKey != null)
            {
                envKey = envKey.Trim();
                string[] envParts = envKey.Contains("=") ? envKey.Split("=") : envKey.Split(" ");
                if (envParts != null && envParts.Length == 2)
                {
                    Environment = envParts[1].Trim();
                }
            }

            Console.WriteLine($"Current environment is: {Environment}");
            bool isSnapRunning = string.Equals(Environment, SnapEnvironmentKey);
            App.IsSnapApp = isSnapRunning;
            BuildAvaloniaApp(isSnapRunning).StartWithClassicDesktopLifetime(args);
            
            Log.Information("Wissance.Zerial application stopped!");
            Log.CloseAndFlush();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp(bool fromSnap)
        {
            // todo(UMV): add profile cfg
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariableSubstitution()
                .Build();
            Services.AddLogging(builder =>
            {
                builder.AddConsole(); // Logs to console window
                builder.AddDebug();   // Logs to IDE Output window
            });
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            Services.AddSingleton<AvaloniaLoggerSink>();
            ServiceProvider sp = Services.BuildServiceProvider();
            
            Log.Information("Wissance.Zerial application started!");
            
            AppBuilder builder = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions
                {
                    RenderingMode = new[] { X11RenderingMode.Software },
                    OverlayPopups = true
                })
                .With(new AvaloniaNativePlatformOptions
                {
                    RenderingMode = new List<AvaloniaNativeRenderingMode>(){AvaloniaNativeRenderingMode.Software}
                })
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI()
                .UseMicrosoftLogging(sp.GetRequiredService<ILoggerFactory>());
            return builder;
        }
        
        public static ServiceCollection Services { get; internal set; }

        public static string Environment { get; set; }

        public const string SnapEnvironmentKey = "snap";
        
        private const string EnvironmentKey = "environment";
        private const string DefaultEnvironment = "win-native";
    }
}