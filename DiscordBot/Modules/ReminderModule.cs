using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace DiscordBot
{
    public class ReminderModule : ModuleBase<SocketCommandContext>
    {
        static readonly List<WrapperClass> Reminders = new List<WrapperClass>();

        [Command("Reminder")]
        [Alias("R")]
        [Summary("Sets a reminder. Format is Reminder <optional dd/MM/yy> <hh:MM> <remindermessage>")]
        public async Task ReminderCommand([Remainder] string args)
        {
            //cursed solution
            var input = args.Split(" ").ToList();
            CultureInfo dk = new CultureInfo("da-DK");

            if (!DateTime.TryParse(input[0], out DateTime time))
            {
                await ReplyAsync($"Invalid format for reminders");
                return;
            }

            if (DateTime.TryParse(input[1], out time))
            {
                time = DateTime.Parse(input[0] + " " + input[1], dk);
                input.RemoveRange(0, 2);

            }
            else
            {
                time = DateTime.Parse(input[0], dk);
                input.RemoveRange(0, 1);

            }
            args = string.Join(" ", input.ToArray());


            TimeZoneInfo TimeInDenmark = TZConvert.GetTimeZoneInfo("Central European Standard Time");
            DateTime DenmarkDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeInDenmark);
            DateTime Now = DateTime.UtcNow.AddHours(DenmarkDateTime.Hour - DateTime.UtcNow.Hour);
            if (time <= Now)
            {
                await ReplyAsync($"Cannot schedule reminders in the past");
                return;
            }
            TimeSpan delay = time - Now;
            Console.WriteLine($"{delay.Days}, {delay.Hours}, {delay.Minutes}, {delay.Seconds}");
            WrapperClass wrapper = new WrapperClass(Context, args);
            Timer ReminderTimer = new Timer(ReminderMessage, wrapper, (int)delay.TotalMilliseconds, Timeout.Infinite);
            wrapper.InternalTimer = ReminderTimer;
            Reminders.Add(wrapper);
            _ = await Context.Channel.SendMessageAsync($"Reminder set at {time:HH:mm}");
        }

        private async void ReminderMessage(object state)
        {
            WrapperClass wrapper = state as WrapperClass;
            await wrapper.Context.Channel.SendMessageAsync($"@everyone {wrapper.Message}");
            wrapper.InternalTimer.Dispose();
            Reminders.Remove(wrapper);
        }

        class WrapperClass
        {
            public WrapperClass(SocketCommandContext context, string message)
            {
                Context = context;
                Message = message;
            }
            public Timer InternalTimer { get; set; }
            public SocketCommandContext Context { get; set; }
            public string Message { get; set; }
        }
    }
}
