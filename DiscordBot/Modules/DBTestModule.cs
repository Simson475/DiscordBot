//using Discord.Commands;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using System;
//using System.Threading.Tasks;

//namespace DiscordBot
//{
//    public class DBTestModule : ModuleBase<SocketCommandContext>
//    {
//        [Command("DB")]
//        [Summary("Test Command")]
//        public async Task DBCommand()
//        {
//            string password = Environment.GetEnvironmentVariable("password");
//            MongoClient client = new MongoClient($"mongodb+srv://dbUser:{password}@botdb.soulu.mongodb.net/<dbname>?retryWrites=true&w=majority");
//            IMongoDatabase database = client.GetDatabase("DiscordBot");
//            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Test");

//            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("student_id", 10000 );
//            BsonDocument test = collection.Find<BsonDocument>(filter).FirstOrDefault();
//            await Context.Channel.SendMessageAsync(test.ToString());
//        }
//    }
//}
