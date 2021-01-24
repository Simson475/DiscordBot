using Discord.Commands;
using System.Data;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MathModule : ModuleBase<SocketCommandContext>
    {
        [Command("Math")]
        [Summary("Computes the given math expression")]
        public async Task MathCommand([Remainder] string math)
        {
            math = math.Replace("`", "");
            DataTable dataTable = new DataTable();
            object result = dataTable.Compute(math, null);
            await ReplyAsync($"Result is {result}");
        }
    }
}
