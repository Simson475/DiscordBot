using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot
{
    // Keep in mind your module **must** be public and inherit ModuleBase.
    // If it isn't, it will not be discovered by AddModulesAsync!
    public class SayModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("Say")]
        [Summary("Echoes a message.")]
        public async Task SayCommand([Remainder][Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }
    }

}
