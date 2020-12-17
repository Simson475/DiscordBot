using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    [Group("Russian Roulette")]
    [Summary("A game of russian roulette!")]
    public class RussianRouletteModule : ModuleBase<SocketCommandContext>
    {
        public int ArmedShots { get; set; } = 0;
        public int TotalShots { get; set; }
        public bool IsGameActive { get; set; } = false;
        [Command("Start")]
        [Summary("Starts a game of russian roulette game")]
        public async Task StartCommand()
        {
            if (IsGameActive)
            {
                await ReplyAsync($"Roulette already started!");
                return;
            }
            ArmedShots = 1;
            TotalShots = 6;
            await ReplyAsync($"Roulette just started");
        }

        [Command("Shoot")]
        [Summary("Fires a shot at yourself!")]
        public async Task ShootCommand()
        {

            if (!IsGameActive) await ReplyAsync("No game is active!");

            var answer = new StringBuilder();
            var slotFired = new Random().Next(1, TotalShots);
            if (slotFired <= ArmedShots)
            {
                answer.AppendLine("You shot yourself!");
                ArmedShots--;
                TotalShots--;
            }
            else
            {
                answer.AppendLine("You still live... for now.");
                TotalShots--;
            }

            if (ArmedShots <= 0)
            {
                answer.AppendLine("The game has ended");
                ArmedShots = 0;
                TotalShots = 0;
                IsGameActive = false;
            }
            await ReplyAsync(answer.ToString());

        }



    }
}
