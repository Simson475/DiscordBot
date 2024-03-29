﻿using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        [Command("Meme")]
        [Alias("Reddit", "R")]
        [Summary("Posts a meme to the channel\nformat is reddit ChosenSubreddit\nAliases: R or Reddit")]
        public async Task MemeCommand(string subreddit = null)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");

            if (!result.StartsWith("["))
            {
                await Context.Channel.SendMessageAsync("This subreddit doesnt exist!");
                return;
            }

            JArray array = JArray.Parse(result);
            JObject post = JObject.Parse(array[0]["data"]["children"][0]["data"].ToString());
            string url = post["url"].ToString();
            if (url.EndsWith(".gifv")) url = url.Remove(url.Length - 1);

            EmbedBuilder builder = new EmbedBuilder()
                .WithImageUrl(url)
                .WithColor(new Color(33, 176, 252))
                .WithTitle(post["title"].ToString())
                .WithUrl($"https://reddit.com{post["permalink"]}")
                .WithFooter($"🗨{post["num_comments"]} ⬆️{post["ups"]}");
            Embed embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
