using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("Ping")]
        [Alias("Latency")]
        [Summary("Gets the latency of the bot")]
        public async Task PingCommand()
        {
            await ReplyAsync($"{Context.User.Mention} Pong! The latency is {Context.Client.Latency}ms");
        }
    }

}
