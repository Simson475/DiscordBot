using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using TimeZoneConverter;

namespace DiscordBot
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        public Timer AbsenceTimer { get; set; }

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _service.CommandExecuted += OnCommandExecuted;
            _client.JoinedGuild += OnJoinedGuild;
            SetupAbsenceTimer();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }


        private async Task OnJoinedGuild(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync($"{_client.CurrentUser.Username} just joined the server!" +
                                                      $" write {_config["prefix"]}help for a list of commands.");
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            int argPos = 0;

            // Should only trigger message created by user if not, return.
            if (!(arg is SocketUserMessage message)) return;
            // Makes sure a bot edit on a user message does not trigger bot.
            if (message.Source != MessageSource.User) return;
            //Bot only triggers if tagged or prefix is correct.
            if (!message.HasStringPrefix(_config["prefix"], ref argPos) &&
                !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnCommandExecuted(Discord.Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"Error: {result}");
        }

        #region absence
        private void SetupAbsenceTimer()
        {
            DateTime now = DateTime.Now;
            TimeZoneInfo TimeInDenmark = TZConvert.GetTimeZoneInfo("Central European Standard Time");
            DateTime DenmarkDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeInDenmark);
            int HoursOffset = now.Hour - DenmarkDateTime.Hour;
            int HoursOfDay = 8;
            DateTime at8 = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HoursOfDay + HoursOffset, 0, 0);
            if (now.Hour >= HoursOfDay - HoursOffset) at8 = at8.AddDays(1);

            TimeSpan timeUntill8 = at8 - now;
            Console.WriteLine("time untill 8 in denmark " + timeUntill8);
            AbsenceTimer = new Timer(MessageAbsence, null, timeUntill8, new TimeSpan(1, 0, 0, 0));
        }

        private async void MessageAbsence(object state)
        {

            Console.WriteLine("Running messageAbsence");
            string password = Environment.GetEnvironmentVariable("password");
            MongoClient client = new MongoClient($"mongodb+srv://dbUser:{password}@botdb.soulu.mongodb.net/<dbname>?retryWrites=true&w=majority");
            IMongoDatabase database = client.GetDatabase("DiscordBot");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Absence");


            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("date", DateTime.Now.ToString("dd/MM/yy"));
            List<BsonDocument> results = collection.Find(filter).ToList();
            IEnumerable<IGrouping<BsonValue, BsonDocument>> groups = results.GroupBy(document => document["server"]).ToList();

            foreach (IGrouping<BsonValue, BsonDocument> group in groups)
            {
                ulong guild = ulong.Parse(group.Key.ToString());
                string absentees = "The Absentees for the day are:\n";
                foreach (BsonDocument element in group)
                {
                    string username = element["username"].ToString();
                    string discriminator = element["discriminator"].ToString();
                    string reason = element["reason"].ToString();
                    //SocketUser actualUser = _client.GetUser(username, discriminator);

                    absentees += $"@{username}: {reason}\n";
                }
                await _client.GetGuild(guild).DefaultChannel.SendMessageAsync(absentees);
            }
            collection.DeleteMany(filter);
        }
        #endregion
    }
}