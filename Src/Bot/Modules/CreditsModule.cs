using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the credits command.
    /// </summary>
    public class CreditsModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the credits command.
        /// </summary>
        /// <returns> . </returns>
        [Command("credits")]
        public async Task CreditsAsync()
        {
            EmbedBuilder embedBuilder = new ()
            {
                Title = "Credits",
                Description = "This bot was programmed by Ludeo\n[Github](https://github.com/Ludeo/)\n" +
                              "[Code](https://github.com/Ludeo/scoresaberbot)\nDiscord: Ludeo#8554\n" +
                              "If you have any questions, feel free to write me a DM on Discord or open a issue on " +
                              "the github repository.\n\nAlso thanks to " +
                              "[ppotatoo](https://github.com/ppotatoo/ssapi/wiki) for making the " +
                              "score saber api wiki :)",
                ThumbnailUrl =
                    "https://cdn.discordapp.com/avatars/311861142114926593/fb1936aec9db8ef087e0627c974fb65e.webp",
                Color = Color.Magenta,
            };

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}