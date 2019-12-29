using FluentCommands.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FluentCommands.Logging
{
    public enum FluentLogLevel { Fatal, Error, Warning, Info, Debug }

    internal class CommandServiceLogger : IFluentLogger
    {
        internal CommandServiceLogger() { }

        async Task IFluentLogger.LogFatal(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Fatal, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.LogError(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Error, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.LogWarning(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Warning, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.LogInfo(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Info, message, e, t).ConfigureAwait(false);
        async Task IFluentLogger.LogDebug(string message, Exception? e, TelegramUpdateEventArgs? t) => await Logging_Internal(FluentLogLevel.Debug, message, e, t).ConfigureAwait(false);

        private async Task Logging_Internal(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null)
        {
            //if ((int)l < (int)CommandService.GlobalConfig.MaximumLogLevel) return;
            var time = DateTime.Now.ToString("hh:mm:ss");
            SetPrefixColors(l);
            var prefixString = GetPrefixString(l, time);
            await Console.Out.WriteLineAsync(prefixString).ConfigureAwait(false);
            SetMessageColors(l);
            await Console.Out.WriteLineAsync($" {m}").ConfigureAwait(false);
            if (e is { }) await Console.Out.WriteLineAsync($" --> {e?.ToString()}").ConfigureAwait(false);
            await Console.Out.WriteLineAsync().ConfigureAwait(false);

            static void SetPrefixColors(FluentLogLevel level)
            {
                switch (level)
                {
                    case FluentLogLevel.Fatal:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case FluentLogLevel.Error:
                        Console.BackgroundColor = ConsoleColor.Red; 
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case FluentLogLevel.Warning:
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case FluentLogLevel.Info:
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case FluentLogLevel.Debug:
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                }
            }
            static string GetPrefixString(FluentLogLevel level, string timeString)
            {
                return level switch
                {
                    FluentLogLevel.Fatal => $" FATAL:  {timeString}  ",
                    FluentLogLevel.Error => $" ERROR:  {timeString}  ",
                    FluentLogLevel.Warning => $"  WARN:  {timeString}  ",
                    FluentLogLevel.Info => $"  INFO:  {timeString}  ",
                    FluentLogLevel.Debug => $" DEBUG:  {timeString}  ",
                    _ => ""
                };
            }
            static void SetMessageColors(FluentLogLevel level)
            {
                switch (level)
                {
                    case FluentLogLevel.Fatal:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case FluentLogLevel.Error:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case FluentLogLevel.Warning:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case FluentLogLevel.Info:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case FluentLogLevel.Debug:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }
            }
        }
    }
}
