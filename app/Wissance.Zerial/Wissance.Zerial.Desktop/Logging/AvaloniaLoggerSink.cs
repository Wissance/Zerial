using System;
using Avalonia.Logging;
using Microsoft.Extensions.Logging;
using AvaloniaLogLevel = Avalonia.Logging.LogEventLevel;
using MicrosoftLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Wissance.Zerial.Desktop.Logging
{
    public class AvaloniaLoggerSink: ILogSink
    {
        public AvaloniaLoggerSink(ILoggerFactory loggerFactory)
        {
            // Creates a dedicated category name in your logs for Avalonia internal diagnostics
            _logger = loggerFactory.CreateLogger("Avalonia.System");
        }

        public bool IsEnabled(AvaloniaLogLevel level, string area)
        {
            return _logger.IsEnabled(ConvertLevel(level));
        }

        public void Log(AvaloniaLogLevel level, string area, object? source, string messageTemplate)
        {
            _logger.Log(ConvertLevel(level), "[{Area}] {Message}", area, messageTemplate);
        }

        public void Log(AvaloniaLogLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
        {
            // Merges the 'area' string into the structured logging argument array
            var args = new object?[propertyValues.Length + 1];
            args[0] = area;
            Array.Copy(propertyValues, 0, args, 1, propertyValues.Length);

            _logger.Log(ConvertLevel(level), "[{Area}] " + messageTemplate, args);
        }

        private static MicrosoftLogLevel ConvertLevel(AvaloniaLogLevel level) => level switch
        {
            AvaloniaLogLevel.Verbose => MicrosoftLogLevel.Debug,
            AvaloniaLogLevel.Information => MicrosoftLogLevel.Information,
            AvaloniaLogLevel.Warning => MicrosoftLogLevel.Warning,
            AvaloniaLogLevel.Error => MicrosoftLogLevel.Error,
            AvaloniaLogLevel.Fatal => MicrosoftLogLevel.Critical,
            _ => MicrosoftLogLevel.None
        };
        
        private readonly ILogger _logger;
    }
}