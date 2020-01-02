using FluentCommands.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FluentCommands.Logging
{

    internal class CommandServiceLogger : IFluentLogger
    {
        private readonly static object _lock = new object();
        private readonly static object _lock2 = new object();
        internal CommandServiceLogger() { }

         Task IFluentLogger.LogFatal(string message, Exception? e, TelegramUpdateEventArgs? t) => Logging_Internal(FluentLogLevel.Fatal, message, e, t);
         Task IFluentLogger.LogError(string message, Exception? e, TelegramUpdateEventArgs? t) => Logging_Internal(FluentLogLevel.Error, message, e, t);
         Task IFluentLogger.LogWarning(string message, Exception? e, TelegramUpdateEventArgs? t) => Logging_Internal(FluentLogLevel.Warning, message, e, t);
         Task IFluentLogger.LogInfo(string message, Exception? e, TelegramUpdateEventArgs? t) => Logging_Internal(FluentLogLevel.Info, message, e, t);
         Task IFluentLogger.LogDebug(string message, Exception? e, TelegramUpdateEventArgs? t) => Logging_Internal(FluentLogLevel.Debug, message, e, t);

        private Task Logging_Internal(FluentLogLevel l, string m, Exception? e = null, TelegramUpdateEventArgs? t = null)
        {
            if ((int)l > (int)CommandService.GlobalConfig.MaximumLogLevel) return Task.CompletedTask;

            lock (_lock)
            {
                Console.Title = "FluentCommands";
                var time = DateTime.Now.ToString("hh:mm:ss");
                SetPrefixColors(l);
                var prefixString = GetPrefixString(l, time);
                Console.WriteLine(prefixString);
                SetMessageColors(l);
                Console.WriteLine($" {m}");
                if (e is { }) Console.WriteLine($" --> {e?.ToString()}");
                Console.WriteLine();
            }

            return Task.CompletedTask;
            static void SetPrefixColors(FluentLogLevel level)
            {
                lock (_lock2)
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
            }
            static string GetPrefixString(FluentLogLevel level, string timeString)
            {
                lock (_lock2)
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
            }
            static void SetMessageColors(FluentLogLevel level)
            {
                lock (_lock2)
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
}
