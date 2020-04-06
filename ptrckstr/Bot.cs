using DSharpPlus;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;
using ptrckstr.Commands;
using ptrckstr.Events;
using System.IO;
using System.Threading.Tasks;

namespace ptrckstr
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextModule Commands { get; private set; }
        public Config Configuration { get; private set; }

        public async Task TaskAsync()
        {
            Configuration = await LoadConfigAsync();
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Configuration.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            });

            Commands = Client.UseCommandsNext(new CommandsNextConfiguration()
            {
                CaseSensitive = false,
                EnableDms = false,
                StringPrefix = Configuration.CommandPrefix
            });

            RegisterCommands();
            RegisterEvents();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private void RegisterCommands()
        {
            Commands.RegisterCommands<GeneralCommands>();
            Commands.RegisterCommands<MinecraftCommands>();
        }

        private void RegisterEvents()
        {
            new ReadyEvent(Client);
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
