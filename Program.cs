using System;
using System.Threading;
using System.Threading.Tasks;
using Bot.Bot;
using Bot.Bot.FileObjects;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Timer = System.Timers.Timer;

namespace Bot
{
    /// <summary>
    ///     Class that gets executed when the program starts, it also starts up the bot.
    /// </summary>
    public class Program
    {
        private static readonly Api.Objects.Api Api = new ();

        /// <summary>
        ///     Gets the timer that is needed for the score tracking.
        /// </summary>
        public static Timer Timer { get; } = new ();

        /// <summary>
        ///     Gets the discord client.
        /// </summary>
        public static DiscordSocketClient Client { get; private set; }

        /// <summary>
        ///     Allows other classes to use the same api so the rate limit doesn't get exceeded.
        /// </summary>
        /// <returns> Returns the Api object from the main class. </returns>
        public static Api.Objects.Api GetApi() => Api;

        private static Task Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            ServiceProvider services = this.ConfigureServices();

            Client = services!.GetRequiredService<DiscordSocketClient>();

            Client.Log += Log;
            services!.GetRequiredService<CommandService>().Log += Log;

            Config config = Config.Default;

            if (string.IsNullOrEmpty(config.Token))
            {
                Console.WriteLine("The bot token is not available in the config.xml file. Add it and restart the bot.");
                Environment.Exit(0);
            }

            string token = config.Token;

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            string prefix = config.Prefix;

            await Client.SetGameAsync($"Beat Saber || {prefix}help");

            await Task.Delay(Timeout.Infinite);
        }

        private ServiceProvider ConfigureServices()
            => new ServiceCollection()
               .AddSingleton<DiscordSocketClient>()
               .AddSingleton<CommandService>()
               .AddSingleton<CommandHandlingService>()
               .BuildServiceProvider();
    }
}