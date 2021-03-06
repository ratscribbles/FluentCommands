**this repo is of a significantly older version that will be updated within the next month or so. big changes coming soon!**

# FluentCommands
A .NET bot framework and command system for Telegram. 

It allows developers to create robust command modules for their bots in an efficient and intuitive way.

**[Click here to join the official Telegram group!](https://t.me/joinchat/PWHYHRBYvBfq4GTZxu0PCA)**

## Prerequisites

A familiarity with [Telegram], [Telegram Bots], and the [.NET Client for Telegram Bot API].

**Requires .NET Core 3.1.**

## Quickstart Guide (full docs coming soon!)

***Before you start, make sure you have a Token generated by the BotFather. @BotFather on Telegram for more info.***

<details>
    <summary><strong>Click to Expand</strong></summary>
<p>
  
FluentCommands supports many different "patterns" in the construction of Command modules. 

The most simple is as follows:

- Create a new Console App (.NET Core) and install the FluentCommands package on NuGet.

- Create a class to contain your Command module, and reference FluentCommands.Commands in your using statements: 

![Class created, using FluentCommands.Commands](https://i.imgur.com/4vvLRfi.png)

- Inherit the `CommandModule<TCommand>` class, referencing your class as `TCommand`:

![CommandModule is referencing MyCommands here.](https://i.imgur.com/Wohyihi.png)

- Create a method with return type (async) `Task`, and one of 5 supported Command Contexts (here, we'll use `MessageContext`):

![This is the structure for a command.](https://i.imgur.com/zkcrEcv.png)

- Use the context to help build your command! Here, we send a simple text message:

![The context contains information about the incoming request(s) going to your client.](https://i.imgur.com/MrfF6aV.png)

_(This can be made *much* more efficient with the `Menu` class, but that's out of scope for this quickstart guide, for now.)_

- Tag your command method with the `CommandAttribute`:

![The command attribute allows you to specify the name of the command for use on Telegram.](https://i.imgur.com/F3egqil.png)

- You're done! All that's left is to start the `CommandService` in your `Main` method (likely located in your `Program` class):

![Please keep your bot token safe](https://i.imgur.com/l24tkww.png)

- Start the application and call the command from your bot.

![We did it!!](https://i.imgur.com/LWjMNtW.png)

Well done! You've created your first Telegram bot with FluentCommands!

This is just the very beginning of what you can do with this library: stay tuned for more in-depth documentation and examples.

</p>
</details>

## Special Thanks

- The [Telegram Bots Team] for creating the .NET client for Telegram.
- [EF Core] and [DSharpPlus] for inspiration on how to approach the design for this project.
- The creator of [Finite.Commands] for helping me with some very specific problems early on.

<!-- ---- -->

[Telegram]: https://www.telegram.org/
[Telegram Bots]: https://core.telegram.org/bots
[.NET Client for Telegram Bot API]: https://core.telegram.org/bots/api
[Telegram Bots Team]: https://github.com/orgs/TelegramBots/people
[EF Core]: https://github.com/aspnet/EntityFrameworkCore
[DSharpPlus]: https://github.com/DSharpPlus/DSharpPlus
[Finite.Commands]: https://github.com/FiniteReality/Finite.Commands
