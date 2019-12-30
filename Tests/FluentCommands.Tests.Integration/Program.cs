using System;
using System.Reflection;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Linq;
using FluentCommands.Logging;
using System.Threading.Tasks;

namespace FluentCommands.Tests.Integration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf1.txt").ElementAt(0);
            CommandService.Start(c =>
            {
                c.AddClient(token);
                c.MaximumLogLevel(FluentLogLevel.Debug);
            });

            //:         explore the MessageType enum for onEvents

            //while (true)
            //{
            //    await CommandService.exposeLogger().LogWarning("Testing...", new Exception("Test exception"));
            //    await Task.Delay(50);
            //    await CommandService.exposeLogger().LogDebug("Testing...Testing...Testing...");
            //    await Task.Delay(50);
            //    await CommandService.exposeLogger().LogError("Testing...Testing...Testing...Testing...Testing...", new Exception("Test exception"));
            //    await Task.Delay(50);
            //    await CommandService.exposeLogger().LogFatal("Testing...Testing...", new Exception("Test exception"));
            //    await Task.Delay(50);
            //    await CommandService.exposeLogger().LogInfo("Testing...", new Exception("Test exception"));
            //    await Task.Delay(50);

            //}

            Console.ReadLine();
        }
    }
}
