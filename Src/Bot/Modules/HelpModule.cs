using System.Configuration;
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
            EmbedBuilder embedBuilder = new()
            {
                Title = "Help",
            };

            Configuration config = HelpFunctions.LoadConfig();
            string prefix = config.AppSettings.Settings["prefix"].Value;

            embedBuilder.AddField(
                prefix + "link [scoresaberid]",
                "Links or updates your discord account to the score saber account",
                true);

            embedBuilder.AddField(
                prefix + "profile | " + prefix + " profile [scoresaberid]",
                "Returns your profile if you have an account linked or returns the profile of the given " +
                "score saber account",
                true);

            embedBuilder.AddField(
                prefix + "recent | " + prefix + " recent [scoresaberid]",
                "Returns the recent play of your linked account or returns the recent play of the given" +
                "score saber account",
                true);

            embedBuilder.AddField(
                prefix + "top5",
                "Returns the profile of the top 5 players",
                true);

            embedBuilder.AddField(
                prefix + "top | " + prefix + "top [scoresaberid]",
                "Returns the top play of your linked account or returns the recent top play of the given" +
                "score saber account",
                true);

            embedBuilder.AddField(
                prefix + "updatename | " + prefix + "updatename[scoresaberid]",
                "Updates your score saber name for the bot or updates the score saber name of the given" +
                "score saber account for the bot",
                true);

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the help command with a specified command.
        /// </summary>
        /// <param name="command"> The command you want help for. </param>
        /// <returns> . </returns>
        [Command("help")]
        public async Task HelpSpecificAsync(string command)
        {
            EmbedBuilder embedBuilder = new ()
            {
                Title = "Help for the " + command + " command",
            };

            Configuration config = HelpFunctions.LoadConfig();
            string prefix = config.AppSettings.Settings["prefix"].Value;

            switch (command)
            {
                case "link":
                    embedBuilder.Description =
                        "With \"" + prefix + "link [scoresaberid]\" you can link the given score saber account to " +
                        "your discord account.\nIf you already linked a score saber account to your discord account, " +
                        "the score saber account will get updated.";

                    break;
                case "profile":
                    embedBuilder.Description =
                        "With \"" + prefix + "profile [scoresaberid]\" you can view the profile of the given score " +
                        "saber account.\nIf you have linked your score saber account to your discord account, you can" +
                        " just use \"" + prefix + "profile\" to view your profile without giving a score saber id.";

                    break;
                case "recent":
                    embedBuilder.Description =
                        "With \"" + prefix + "recent [scoresaberid]\" you can view the most recent play of the " +
                        "given score saber account.\nIf you already linked your score saber account to your discord " +
                        "account, you can just use \"" + prefix + "recent\" to view your most recent play without " +
                        "giving a score saber id.";

                    break;
                case "top5":
                    embedBuilder.Description =
                        "With \"" + prefix + "top5\" you can view the profile of the top 5 ranked score saber players.";

                    break;
                case "top":
                    embedBuilder.Description =
                        "With \"" + prefix + "top [scoresaberid]\" you can view the top play of the given score " +
                        "saber account.\nIf you already linked your score saber account to your discord account, " +
                        "you can just use \"" + prefix + "top\" to view your top play without giving a score saber id.";

                    break;
                case "updatename":
                    embedBuilder.Description =
                        "With \"" + prefix + "updatename [scoresaberid]\" you can update the username of the given" +
                        "score saber account for the bot.\nIf you already linked your score saber account to your" +
                        " discord account, you can just use \"" + prefix + "updatename\" to update your score saber " +
                        "name for the bot.\n\nNOTE: This will only update your score saber name for the BOT. It will " +
                        "not update your score saber name for score saber itself!";

                    break;
                case "credits":
                    embedBuilder.Description =
                        "\"" + prefix + "credits\" will show information about the creator of the bot.";

                    break;
                default:
                    embedBuilder.Description =
                        "This command does not exist. Use \"" + prefix + "help\" to see a list of commands.";

                    break;
            }

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}