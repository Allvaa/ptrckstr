using DSharpPlus;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
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

            Client.Ready += OnClientReady;
            Client.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().Equals("haee"))
                await e.Message.RespondAsync("Hi");
            };

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            Console.WriteLine($"ppppppppppppppppppppppppppppppppppppp {e.Client.CurrentUser.Username}");
            return Task.CompletedTask;
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
