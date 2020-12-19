using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{

    public class Program
    {
        public static async Task Main(string[] args) => await Startup.RunAsync(args);
        //public static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        //    private DiscordSocketClient _client;
        //    private CommandService _commands;
        //    private CommandHandler _commandHandler;

        //    public async Task MainAsync()
        //    {

        //        _client = new DiscordSocketClient();
        //        _commands = new CommandService();

        //        _client.Log += Log;
        //        _commandHandler = new CommandHandler(_client, _commands);

        //        //discord token
        //        string token = Environment.GetEnvironmentVariable("Token");
        //        //string token = File.ReadAllText("./env.txt");

        //        await _commandHandler.InstallCommandsAsync();
        //        await _client.LoginAsync(TokenType.Bot, token);
        //        await _client.StartAsync();

        //        // Block this task until the program is closed.
        //        await Task.Delay(-1);
        //    }

        //    private Task Log(LogMessage msg)
        //    {
        //        Console.WriteLine(msg.ToString());
        //        return Task.CompletedTask;
        //    }
    }
}
