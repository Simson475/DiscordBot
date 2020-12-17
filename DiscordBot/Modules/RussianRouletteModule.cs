using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot
{
    [Group("Russian Roulette")]
    [Summary("A game of russian roulette!")]
    public class RussianRouletteModule : ModuleBase<SocketCommandContext>
    {
        public class StartModule : ModuleBase<SocketCommandContext>
        {
            [Command("Start")]
            [Summary("Starts the game")]
            public async Task StartCommand()
            {

                await ReplyAsync($"Roulette just started");
            }

        }

    }
}
