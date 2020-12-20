using Discord.WebSocket;
using System.Collections.Generic;

namespace DiscordBot
{
    public class RouletteGame
    {
        public int Shots { get; set; }
        public int Armed { get; set; }
        public ulong GuildID { get; set; }
        public List<SocketUser> ShotUsers { get; set; } = new List<SocketUser>();
    }
}