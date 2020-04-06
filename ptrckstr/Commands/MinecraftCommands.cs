using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using System;
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
            if (uuid == null) await ctx.RespondAsync("gada");
            await ctx.RespondAsync(uuid);
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
    }
}
