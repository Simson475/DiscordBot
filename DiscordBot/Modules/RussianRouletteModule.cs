using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{
    [Group("Russian Roulette")]
    [Alias("RR")]
    public class RussianRouletteModule : ModuleBase<SocketCommandContext>
    {
        private static List<RouletteGame> ActiveGames { get; set; } = new List<RouletteGame>();

        [Command("Start")]
        [Summary("Starts a Russian Roulette game!\n should be followed by amount of shots and amount armed (alias RR)")]
        public async Task StartCommand([Remainder] string arg)
        {
            string[] args = arg.Split(" ");
            //consider making clearer error messages
            if (!int.TryParse(args[0], out int shots) ||
                !int.TryParse(args[1], out int armed) ||
                shots <= armed ||
                shots < 0 ||
                armed < 0)
            {
                await ReplyAsync("Invalid amount of shots");
                return;
            }

            RouletteGame game = ActiveGames.Find(x => x.GuildID == Context.Guild.Id);
            if (game != null)
            {
                await ReplyAsync("Game is already active!");
                return;
            }
            ActiveGames.Add(new RouletteGame() { Shots = shots, Armed = armed, GuildID = Context.Guild.Id });
            await ReplyAsync($"Games is started with {shots} shots where {armed} shots are armed");
        }

        [Command("End")]
        [Summary("Ends the active game of Russian Roulette")]
        public async Task EndCommand()
        {
            int ended = ActiveGames.RemoveAll(x => x.GuildID == Context.Guild.Id);
            await ReplyAsync(ended == 0 ? "No game is active" : "Game ended");
        }

        [Command("Shoot")]
        [Summary("Fires the gun at yourself!")]
        public async Task ShootCommand()
        {
            string message = "";
            RouletteGame game = ActiveGames.Find(x => x.GuildID == Context.Guild.Id);

            if (game == null) message += "No game is active, start one with RR start";
            else if (game.ShotUsers.Find(x => x == Context.User) != null) message += $"{Context.User.Mention} is already dead.";
            else
            {
                Random random = new();
                int shotFired = random.Next(1, game.Shots);
                game.Shots--;

                if (shotFired > game.Armed) message += $"{Context.User.Mention} fired a shot, but nothing happened.\n";
                else
                {
                    game.Armed--;
                    game.ShotUsers.Add(Context.User);
                    message += $"{Context.User.Mention} was shot!\n";
                }
                // consider if it should only end if armed == 0
                if (game.Armed == 0 ||
                    game.Shots == 0 ||
                    game.Armed >= game.Shots)
                {
                    ActiveGames.RemoveAll(x => x.GuildID == Context.Guild.Id);
                    message += $"game ended since there are {game.Shots} shots left with {game.Armed} armed shots";
                }
                else message += $"There are {game.Shots} shots left, {game.Armed} are still armed.";
            }
            await ReplyAsync(message);
        }
    }
}
