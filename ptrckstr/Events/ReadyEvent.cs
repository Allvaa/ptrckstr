using DSharpPlus;
using Lavalink4NET;
using System;
using System.Threading.Tasks;

namespace ptrckstr.Events
{
    class ReadyEvent
    {
        public ReadyEvent(DiscordClient client)
        {
            client.Ready += (e) =>
            {
                Console.WriteLine($"Logged in as {e.Client.CurrentUser.Username}#{e.Client.CurrentUser.Discriminator}.");
                return Task.CompletedTask;
            };
        }
    }
}
