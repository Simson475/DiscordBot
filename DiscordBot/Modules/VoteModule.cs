using Discord;
using Discord.Commands;
using Discord.Rest;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class VoteModule : ModuleBase<SocketCommandContext>
    {
        [Command("Vote")]
        [Summary("Calls a vote. Format is Vote vote subject \"option1\" \"option2\"\n max 9 choices")]
        public async Task VoteCommand([Remainder] string args)
        {

            string[] numbers = new string[] { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:" };
            var emojis = new Emoji[] { new Emoji("1️⃣"), new Emoji("2️⃣"), new Emoji("3️⃣"), new Emoji("4️⃣"), new Emoji("5️⃣"), new Emoji("6️⃣"), new Emoji("7️⃣"), new Emoji("8️⃣"), new Emoji("9️⃣"), new Emoji("") };



            List<string> choices = args.Split("\"")
                                       .Select(x => x.Trim())
                                       .Where(x => !String.IsNullOrEmpty(x))
                                       .ToList();


            string title = choices[0];
            choices.RemoveAt(0);

            EmbedBuilder builder = new EmbedBuilder()
                .WithColor(new Color(33, 176, 252))
                .WithTitle(title);
            if (numbers.Length < choices.Count)
            {
                await Context.Channel.SendMessageAsync("Too many options. please limit yourself to 9");
                return;
            }
            for (int i = 0; i < choices.Count; i++)
            {
                builder.AddField(numbers[i], choices[i]);
            }

            Embed embed = builder.Build();
            RestUserMessage message = await Context.Channel.SendMessageAsync(null, false, embed);

            for (int i = 0; i < choices.Count; i++)
            {
                await message.AddReactionAsync(emojis[i]);
            }
        }
    }
}
