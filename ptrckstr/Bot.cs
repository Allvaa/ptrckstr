using DSharpPlus;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using ptrckstr.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ptrckstr
{
    class Bot
    {
        public DiscordClient Client { get; private set; }

        public async Task TaskAsync()
        {
            Config Configuration = await LoadConfigAsync();
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Configuration.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            });

            new ReadyEvent(Client);

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        public async Task<Config> LoadConfigAsync()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = await r.ReadToEndAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<Config>(json);
            }
        }
    }
}
