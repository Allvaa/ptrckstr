using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace ptrckstr
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public async Task TaskAsync()
        {
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = "kowakoawkoawkoawko",
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
    }
}
