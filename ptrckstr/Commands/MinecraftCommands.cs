using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ptrckstr.Commands
{
    class MinecraftCommands
    {
        [Command("minecrafthead")]
        [Description("Get minecraft skin head")]
        [Aliases("mchead")]
        public async Task MinecraftHead(CommandContext ctx, string Username)
        {
            string uuid = await GetUuidASync(Username);
            if (Username == null || uuid == null)
            {
                await ctx.RespondAsync("Player not found.");
                return;
            }

            DiscordEmbed Embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blurple)
                .WithImageUrl($"https://crafatar.com/renders/head/{uuid}?overlay")
                .Build();

            await ctx.RespondAsync(embed: Embed);
        }

        [Command("minecraftserver")]
        [Description("Get minecraft server info")]
        [Aliases("mcserver")]
        public async Task MinecraftServer(CommandContext ctx, string Hostname)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.mcsrvstat.us/2/{Hostname}");
            if (Hostname == null || !response.IsSuccessStatusCode)
            {
                await ctx.RespondAsync("Couldn't find server.");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            McServer mcsi = JObject.Parse(responseBody).ToObject<McServer>();
            DiscordEmbed Embed = null;
            if (!mcsi.Online)
            {
                Embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blurple)
                .WithAuthor(Hostname.ToLower(), $"https://mcsrvstat.us/server/{Hostname.ToLower()}", $"https://api.mcsrvstat.us/icon/{Hostname.ToLower()}")
                .WithDescription("Server is offline.")
                .Build();
            } else
            {
                Embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Blurple)
                .WithAuthor(Hostname.ToLower(), $"https://mcsrvstat.us/server/{Hostname.ToLower()}", $"https://api.mcsrvstat.us/icon/{Hostname.ToLower()}")
                .WithDescription($@"
• Address: {mcsi.Ip}:{mcsi.Port}
• Software: {mcsi.Software} {mcsi.Version}
• Players: {mcsi.Players.Online}/{mcsi.Players.Max}
```ml
{string.Join("\n", mcsi.Motd.Clean)}
```
")
                .Build();
            }
            await ctx.RespondAsync(embed: Embed);
        }

        private async Task<string> GetUuidASync(string Username)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{Username}");
            string responseBody = await response.Content.ReadAsStringAsync();
            if (responseBody.Length <= 0) return null;
            return JsonConvert.DeserializeObject<GetUuidResp>(responseBody).Id;
        }

        private struct GetUuidResp
        {
            [JsonProperty("id")]
            public string Id { get; private set; }

            [JsonProperty("name")]
            public string Name { get; private set; }
        }

        private struct McServer
        {
            [JsonProperty("online")]
            public bool Online { get; private set; }
            [JsonProperty("ip")]
            public string Ip { get; private set; }
            [JsonProperty("port")]
            public string Port { get; private set; }
            [JsonProperty("motd")]
            public McServerMotd Motd { get; private set; }
            [JsonProperty("players")]
            public McServerPlayers Players { get; private set; }
            [JsonProperty("version")]
            public string Version { get; private set; }
            [JsonProperty("hostname")]
            public string Hostname { get; private set; }
            [JsonProperty("software")]
            public string Software { get; private set; }


            public struct McServerMotd
            {
                [JsonProperty("clean")]
                public string[] Clean { get; private set; }
            }

            public struct McServerPlayers
            {
                [JsonProperty("online")]
                public int Online { get; private set; }
                [JsonProperty("max")]
                public int Max { get; private set; }
            }
        }
    }
}
