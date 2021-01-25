using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the help command.
    /// </summary>
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the help command.
        /// </summary>
        /// <returns> . </returns>
        [Command("help")]
        public async Task HelpAllAsync()
        {
            EmbedBuilder embedBuilder = new ();
            embedBuilder.Description = "Test";
            embedBuilder.Title = "test";

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}