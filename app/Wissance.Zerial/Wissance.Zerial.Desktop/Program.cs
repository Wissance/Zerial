using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Linq;

namespace Wissance.Zerial.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        string env = args.FirstOrDefault(a => a.Contains(EnvironmentKey));
        Environment = OtherEnvironmentKey;
        if (env != null)
        {
            string[] parts = env.Split("=");
            if (parts.Length == 2)
            {
                Environment = parts[1].ToLower() == SnapEnvironmentKey ? SnapEnvironmentKey : OtherEnvironmentKey;
            }
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    
    public static string Environment { get; set; }

    public const string SnapEnvironmentKey = "snap";
    public const string OtherEnvironmentKey = "other";
    private const string EnvironmentKey = "environment";
}