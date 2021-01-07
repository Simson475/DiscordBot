using Discord.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AbsenceModule : ModuleBase<SocketCommandContext>
    {
        [Command("Absent")]
        [Summary("Marks the user as absent for the given date. format is Absent dd/MM/yy reason")]
        public async Task AbsentCommand([Remainder] string args)
        {
            List<string> argumentsSplit = args.Split(" ").ToList();
            DateTime date = DateTime.Parse(argumentsSplit[0], CultureInfo.CreateSpecificCulture("da-DK"));


            if (argumentsSplit.Count < 2)
            {
                await ReplyAsync("please supply both date and reason" + argumentsSplit.Count);
                return;
            }
            if (date < DateTime.UtcNow)
            {
                await ReplyAsync("Cannot add absence to past dates");
                return;
            }

            argumentsSplit.RemoveAt(0);
            string reason = String.Join(" ", argumentsSplit);
            string password = Environment.GetEnvironmentVariable("password");
            MongoClient client = new MongoClient($"mongodb+srv://dbUser:{password}@botdb.soulu.mongodb.net/<dbname>?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("DiscordBot");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Absence");

            BsonDocument entry = new BsonDocument()
            {
                {"server", Context.Guild.Id.ToString() },
                {"username", Context.User.Username},
                {"discriminator", Context.User.Discriminator },
                {"date", date.ToString("dd/MM/yy") },
                {"reason", reason }
            };
            collection.InsertOne(entry);

            await Context.Channel.SendMessageAsync("Absense added.");
        }

    }
}
