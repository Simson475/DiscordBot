using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
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
            //Creates new commandservices which can gain information about all modules.
            CommandService _commands = new();
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
            List<CommandInfo> commands = _commands.Commands.ToList();

            //Builds an embedded list with all commands and their summary
            EmbedBuilder embedBuilder = new();
            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string group = command.Module.Group + " ";
                string embedFieldText = command.Summary ?? "No description available\n";
                embedBuilder.AddField($"{group} {command.Name}", embedFieldText);
            }
            await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
    }

}
