using Avalonia;
using Microsoft.Extensions.Logging;
using Wissance.Zerial.Desktop.Logging;

namespace Wissance.Zerial.Desktop.Extensions
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder UseMicrosoftLogging(this AppBuilder builder, ILoggerFactory loggerFactory)
        {
            // Redirects Avalonia's global log output to your MS Logging Factory
            Avalonia.Logging.Logger.Sink = new AvaloniaLoggerSink(loggerFactory);
            return builder;
        }
    }
}