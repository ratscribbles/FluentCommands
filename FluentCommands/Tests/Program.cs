using System;
using System.Reflection;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Linq;
using FluentCommands.Logging;
using System.Threading.Tasks;
using FluentCommands.Commands;

namespace FluentCommands.Tests.Integration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //CommandService.Start(c =>
            //{
            //    c.AddClient(Tokens.Token);
            //    c.MaximumLogLevel(FluentLogLevel.Debug);
            //    c.SwallowCriticalExceptions();
            //});
            CommandService.Start(new CommandServiceConfigBuilder(c =>
            {

            }));

            //:         explore the MessageType enum for onEvents

            Console.ReadLine();
        }
    }
}
