using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Bot
{
    /// <summary>
    ///     Service that handles the commands for discord.
    /// </summary>
    public class CommandHandlingService
    {
        private static DiscordSocketClient client;
        private static CommandService commands;
        private static IServiceProvider services;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandHandlingService"/> class.
        /// </summary>
        /// <param name="services"> The service provider of the services. </param>
        public CommandHandlingService(IServiceProvider services)
        {
            client = services!.GetRequiredService<DiscordSocketClient>();
            commands = services.GetRequiredService<CommandService>();
            CommandHandlingService.services = services;

            commands.CommandExecuted += this.CommandExecutedAsync;
            client.MessageReceived += this.MessagedReceivedAsync;
        }

        /// <summary>
        ///     Initializes the CommandHandlingService.
        /// </summary>
        /// <returns> Adds the services to the commands. </returns>
        public async Task InitializeAsync() => await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

        private async Task MessagedReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            int argPos = 0;

            if (!message.HasStringPrefix(HelpFunctions.LoadConfig().AppSettings.Settings["prefix"].Value, ref argPos))
            {
                return;
            }

            SocketCommandContext context = new (client, message);

            await commands.ExecuteAsync(context, argPos, services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                LogMessage notSpecifiedLog = new (LogSeverity.Error, "Command", context.Message.ToString());
                Console.WriteLine(notSpecifiedLog + " || Command doesn't exist");

                return;
            }

            if (result.IsSuccess)
            {
                LogMessage successLog = new (LogSeverity.Info, "Command", context.Message.ToString());
                Console.WriteLine(successLog);

                return;
            }

            await context.Channel.SendMessageAsync($"error: {result}");

            LogMessage errorLog = new (LogSeverity.Error, "Command", context.Message.ToString());

            Console.WriteLine(errorLog + " || " + result);
        }
    }
}