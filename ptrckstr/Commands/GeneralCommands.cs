using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace ptrckstr.Commands
{
    class GeneralCommands
    {
        [Command("ping")]
        [Description("WebSocket ping.")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.RespondAsync($"Pong! `{ctx.Client.Ping}ms`");
        }
    }
}
