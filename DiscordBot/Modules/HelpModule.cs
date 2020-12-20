using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {

        [Command("Help")]
        [Summary("Provides a list of commands for the bot")]
        public async Task HelpCommand()
        {
            CommandService _commands = new CommandService();
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
            List<CommandInfo> commands = _commands.Commands.ToList();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string test = command.Module.Group;
                if (test != "")
                {

                }
                string embedFieldText = command.Summary ?? "No description available\n";
                embedBuilder.AddField(command.Name, embedFieldText);
            }

            await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
    }

    public class MathModule : ModuleBase<SocketCommandContext>
    {
        [Command("Math")]
        [Summary("Computes the given math expression")]
        public async Task MathCommand([Remainder] string math)
        {
            math = math.Replace("`", "");
            DataTable dataTable = new DataTable();
            var result = dataTable.Compute(math, null);
            await ReplyAsync($"Result is {result}");
        }
    }

}
