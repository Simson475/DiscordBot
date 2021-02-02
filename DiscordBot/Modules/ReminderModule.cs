using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace DiscordBot
{
    public class ReminderModule : ModuleBase<SocketCommandContext>
    {
        [Command("Reminder")]
        [Alias("R")]
        [Summary("Sets a reminder. Format is Reminder 12:34 remindermessage\n" +
                 "can only be used on the same day")]
        public async Task ReminderCommand(DateTime time, [Remainder] string args)
        {
            TimeZoneInfo TimeInDenmark = TZConvert.GetTimeZoneInfo("Central European Standard Time");
            DateTime DenmarkDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeInDenmark);
            DateTime Now = DateTime.UtcNow.AddHours(DenmarkDateTime.Hour - DateTime.UtcNow.Hour);
            if (time <= Now)
            {
                await ReplyAsync($"Cannot schedule reminders in the past");
                return;
            }
            TimeSpan delay = time - Now;
            Timer ReminderTimer = new Timer(ReminderMessage, new WrapperClass(Context, args), (int)delay.TotalMilliseconds, Timeout.Infinite);
            await Context.Channel.SendMessageAsync($"Reminder set at {time:HH:mm}");
        }

        private void ReminderMessage(object state)
        {
            WrapperClass wrapper = state as WrapperClass;
            _ = wrapper.Context.Channel.SendMessageAsync($"@everyone {wrapper.Message}");
        }

        class WrapperClass
        {
            public WrapperClass(SocketCommandContext context, string message)
            {
                Context = context;
                Message = message;
            }
            public SocketCommandContext Context { get; set; }
            public string Message { get; set; }
        }
    }
}
