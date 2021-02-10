using System;
using System.IO;
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
            CommandHandlingService.services = services;

            client = services!.GetRequiredService<DiscordSocketClient>();
            commands = services.GetRequiredService<CommandService>();

            commands.CommandExecuted += this.CommandExecutedAsync;
            client.MessageReceived += this.MessagedReceivedAsync;
        }

        /// <summary>
        ///     Initializes the CommandHandlingService.
        /// </summary>
        /// <returns> An empty task. </returns>
        public async Task InitializeAsync() => await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

        private async Task MessagedReceivedAsync(SocketMessage rawMessage)
        {
            int argPos = 0;

            if (rawMessage is SocketUserMessage { Source: MessageSource.User } message &&
                message.HasStringPrefix(Config.Default.Prefix, ref argPos))
            {
                SocketCommandContext context = new (client, message);

                await commands.ExecuteAsync(context, argPos, services);
            }
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            string message = context.Message.ToString();

            if (!command.IsSpecified)
            {
                LogMessage notSpecifiedLog = new (LogSeverity.Error, "Command", message);
                Console.WriteLine(notSpecifiedLog + " || Command doesn't exist");
            }
            else if (result.IsSuccess)
            {
                LogMessage successLog = new (LogSeverity.Info, "Command", message);
                Console.WriteLine(successLog);
            }
            else
            {
                await context.Channel.SendMessageAsync($"error: {result}");

                LogMessage errorLog = new (LogSeverity.Error, "Command", message);
                Console.WriteLine(errorLog + " || " + result);
            }
        }
    }
}