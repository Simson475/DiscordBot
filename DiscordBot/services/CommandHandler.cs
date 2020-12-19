using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{

    class CommandHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static CommandService _commands;
        public static IConfigurationRoot _config;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            int argPos = 0;

            if (!(message.HasStringPrefix(_config["prefix"], ref argPos) ||
                message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            SocketCommandContext context = new SocketCommandContext(_discord, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.Error);
            }
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            Console.WriteLine($"The prefix is {_config["prefix"]}");
            return Task.CompletedTask;
        }
    }
    //    public class CommandHandler
    //    {
    //        private readonly DiscordSocketClient _client;
    //        private readonly CommandService _commands;
    //        private readonly IServiceProvider _services;

    //        // Retrieve client and CommandService instance via ctor
    //        public CommandHandler(DiscordSocketClient client, CommandService commands)
    //        {
    //            _commands = commands;
    //            _client = client;
    //            _services = new ServiceCollection()
    //                .AddSingleton(_client)
    //                .AddSingleton(_commands)
    //                .BuildServiceProvider();
    //        }

    //        public async Task InstallCommandsAsync()
    //        {
    //            // Hook the MessageReceived event into our command handler
    //            _client.MessageReceived += HandleCommandAsync;

    //            // Here we discover all of the command modules in the entry 
    //            // assembly and load them. Starting from Discord.NET 2.0, a
    //            // service provider is required to be passed into the
    //            // module registration method to inject the 
    //            // required dependencies.
    //            //
    //            // If you do not use Dependency Injection, pass null.
    //            // See Dependency Injection guide for more information.
    //            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
    //                                            services: _services);
    //        }

    //        private async Task HandleCommandAsync(SocketMessage messageParam)
    //        {
    //            // Don't process the command if it was a system message
    //            if (!(messageParam is SocketUserMessage message)) return;

    //            // Create a number to track where the prefix ends and the command begins
    //            int argPos = 0;

    //            // Determine if the message is a command based on the prefix and make sure no bots trigger commands from other bots
    //            if (!(message.HasCharPrefix('#', ref argPos) ||
    //                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
    //                message.Author.IsBot)
    //                return;

    //            // Create a WebSocket-based command context based on the message
    //            SocketCommandContext context = new SocketCommandContext(_client, message);

    //            // Execute the command with the command context we just
    //            // created, along with the service provider for precondition checks.
    //            await _commands.ExecuteAsync(
    //                context: context,
    //                argPos: argPos,
    //                services: _services);
    //        }
    //    }
}
