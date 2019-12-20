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

namespace FluentCommands.Tests.Unit
{
    public class TestingModule : CommandModule<TestingModule>
    {
        [Command("e")]
        [Step(-1)]
        public async Task<IStep> PwiOOP(TelegramBotClient client, MessageEventArgs e)
        {
            var lastStep = await Step.LastStep(client, e);
            if (lastStep.CurrentStepNumber == 1)
            {
                return Step.Success();
            }
            else return Step.Failure();
        }
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
                return Step.Success(async () =>
                {
                    await client.SendTextMessageAsync(e.Message.Chat.Id, "1 test successful. enter pee for step success");
                });
            }
            else
            {
                return Step.Failure(async () => { await client.SendTextMessageAsync(e.Message.Chat.Id, "failed. please enter pee for step success."); }).RedoThisStep();
            }
        }
        [Command("e")]
        [Step(2)]
        public async Task<IStep> PwOaOP(TelegramBotClient client, MessageEventArgs e)
        {
            var lastStep = await Step.LastStep(client, e);
            if (lastStep.PreviousStepAction == StepAction.Next) ;

            if (lastStep.PreviousStepResult == StepResult.Failure) await client.SendTextMessageAsync(e.Message.Chat.Id, "im sad");

            if (e.Message.Text == "pee")
            {
                return Step.Success(async () => { await client.SendTextMessageAsync(e.Message.Chat.Id, "2 test successful. enter pee for step success"); });
            }
            else if (e.Message.Text == "go to start")
            {
                return Step.Success(async () => { await client.SendTextMessageAsync(e.Message.Chat.Id, "returning to start"); }).ReturnToStart();
            }
            else if (e.Message.Text == "undo")
            {
                return Step.Success(async () => { await client.SendTextMessageAsync(e.Message.Chat.Id, "undoing"); }).GoToPrevious();
            }
            else
            {
                return Step.Failure(async () => { await client.SendTextMessageAsync(e.Message.Chat.Id, "no"); }).RedoThisStep();
            }

            //: Have users enforce lastStep if they want a different result when revisiting Steps in the past, etc. it should just be a giant switch statement really, lol.
        }
        [Command("e")]
        [Step(3)]
        public async Task<IStep> PwfOOP(TelegramBotClient client, MessageEventArgs e)
        {
            switch (e.Message.Text)
            {
                case "pee":
                    return Step.Success(async () =>
                    {
                        await client.SendTextMessageAsync(e.Message.Chat.Id, "3 test successful. you did it");
                    });
                case "penis":
                    return Step.Failure(async () =>
                    {
                        for (int i = 0; i < 25; i++)
                            await Console.Out.WriteLineAsync("PEE");

                        await client.SendTextMessageAsync(e.Message.Chat.Id, "YOU PIECE OF SHIT");
                        await client.SendTextMessageAsync(e.Message.Chat.Id, "A");
                        await client.SendTextMessageAsync(e.Message.Chat.Id, "   A");
                        await client.SendTextMessageAsync(e.Message.Chat.Id, "       A");
                    });
                default:
                    return Step.Failure(async () =>
                    {
                        await client.SendTextMessageAsync(e.Message.Chat.Id, "no");
                    }).RedoThisStep();
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
