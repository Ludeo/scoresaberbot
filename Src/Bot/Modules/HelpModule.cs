using System;
using System.Threading.Tasks;
using Bot.Bot.FileObjects;
using Bot.Bot.Modules.Enums;
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
        /// <returns> An empty task. </returns>
        [Command("help")]
        public async Task HelpAllAsync()
        {
            EmbedBuilder embedBuilder = new ()
            {
                Title = "Help",
                Color = Color.Gold,
            };

            Config config = Config.Default;
            string prefix = config.Prefix;

            embedBuilder.AddField(prefix + "credits", "Shows the credits for the bot", true);

            embedBuilder.AddField(
                prefix + "leaderboard",
                "Shows a score saber leaderboard of the discord with all players, that linked their accounts",
                true);

            embedBuilder.AddField(
                prefix + "link [scoresaberid]",
                "Links or updates your discord account to the score saber account",
                true);

            embedBuilder.AddField(
                prefix + "profile | " + prefix + "profile [scoresaberid]",
                "Returns your profile if you have an account linked or returns the profile of the given score saber account",
                true);

            embedBuilder.AddField(
                prefix + "recent | " + prefix + "recent [scoresaberid]",
                "Returns the most recent play of your linked account or returns the most recent play of the given score saber account",
                true);

            embedBuilder.AddField(prefix + "top5", "Returns the profile of the top 5 players", true);

            embedBuilder.AddField(
                prefix + "top | " + prefix + "top [scoresaberid]",
                "Returns the top play of your linked account or returns the top play of the given score saber account",
                true);

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the help command with a specified command.
        /// </summary>
        /// <param name="command"> The command you want help for. </param>
        /// <returns> An empty task. </returns>
        [Command("help")]
        public async Task HelpSpecificAsync(Command command)
        {
            EmbedBuilder embedBuilder = new ()
            {
                Title = $"Help for the {command} command",
                Color = Color.Gold,
            };

            Config config = Config.Default;
            string prefix = config.Prefix;

            string alreadyLinked = Environment.NewLine + "If you already linked a score saber account to your discord account,";

            embedBuilder.Description = command switch
            {
                Command.Link => $"With \"{prefix}link [scoresaberid]\" you can link the given score saber account to your discord account. " +
                                $"{alreadyLinked} the score saber account will get updated.",

                Command.Profile => $"With \"{prefix}profile [scoresaberid]\" you can view the profile of the given score saber account. " +
                                   $"{alreadyLinked} you can just use \"{prefix}profile\" to view your profile without giving a score saber id.",

                Command.Recent => $"With \"{prefix}recent [scoresaberid]\" you can view the most recent play of the given score saber account. " +
                                  $"{alreadyLinked} you can just use \"{prefix}recent\" to view your most recent play without giving a score " +
                                  "saber id.",

                Command.Top5 => $"With \"{prefix}top5\" you can view the profiles of the top 5 ranked score saber players.",

                Command.Top => $"With \"{prefix}top [scoresaberid]\" you can view the top play of the given score saber account. {alreadyLinked} " +
                               $"you can just use \"{prefix}top\" to view your top play without giving a score saber id.",

                Command.Credits => $"\"{prefix}credits\" will show information about the creator of the bot.",

                Command.Leaderboard => $"\"{prefix}leaderboard\" will show a leaderboard of the top10 score saber players of the server, that " +
                                       "have their accounts linked. It will also show your position on the leaderboard",

                var _ => $"This command does not exist. Use \"{prefix}help\" to see a list of commands.",
            };

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}