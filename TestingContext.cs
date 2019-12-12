using System;
using System.Collections.Generic;
using System.Text;
using FluentCommands.Builders;
using FluentCommands.Attributes;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using FluentCommands.Logging;
using FluentCommands.Menus;
using FluentCommands.CommandTypes.Steps;

namespace FluentCommands
{
    public class TestingContext : CommandModule<TestingContext>
    {
        [Command("e")]
        [Step(0)]
        public async Task<IStep> PwOOP(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "test successful. enter pee for step success");
            return Step.Success();
        }
        [Command("e")]
        [Step(1)]
        public async Task<IStep> PweOOP(TelegramBotClient client, MessageEventArgs e)
        {
            if (e.Message.Text == "pee")
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "1 test successful. enter pee for step success");
                return Step.Success();
            }
            else
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "no");
                return Step.Failure().RedoThisStep();
            }
        }
        [Command("e")]
        [Step(2)]
        public async Task<IStep> PwOaOP(TelegramBotClient client, MessageEventArgs e)
        {
            var lastStep = await Step.LastStep(e);
            if(lastStep.PreviousStepAction == StepAction.Next)
                await client.SendTextMessageAsync(e.Message.Chat.Id, "kaka poopoo");

            if (e.Message.Text == "pee")
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "2 test successful. enter pee for step success");
                return Step.Success();
            }
            else if (e.Message.Text == "go to start")
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "returning to start");
                return Step.Success().ReturnToStart();
            }
            else if (e.Message.Text == "undo")
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "undoing");
                return Step.Success().GoToPrevious();
            }
            else
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "no");
                return Step.Failure().RedoThisStep();
            }
        }
        [Command("e")]
        [Step(3)]
        public async Task<IStep> PwfOOP(TelegramBotClient client, MessageEventArgs e)
        {
            if (e.Message.Text == "pee")
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "3 test successful. you did it");
                return Step.Success();
            }
            else
            {
                await client.SendTextMessageAsync(e.Message.Chat.Id, "no");
                return Step.Failure().RedoThisStep();
            }
        }

        [Command("pee")]
        public async Task Wow(TelegramBotClient client, MessageEventArgs e)
        {
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
            await client.SendTextMessageAsync(e.Message.Chat.Id, "POGGERS");
        }
    }
}
