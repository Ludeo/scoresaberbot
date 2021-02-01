using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Bot.Api.Objects;
using Discord;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the profile command.
    /// </summary>
    public class ProfileModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that will get executed when someone uses the profile command with a playerId specified.
        /// </summary>
        /// <param name="playerId"> The score saber id of the player that should be displayed. </param>
        /// <returns> . </returns>
        [Command("profile")]
        public async Task ProfileAsync(long playerId)
        {
            Api.Objects.Api api = Program.GetApi();

            Player player = await api.GetPlayerAsync(playerId);

            Configuration playerNames = HelpFunctions.LoadPlayerNames();

            if (playerNames.AppSettings.Settings.AllKeys!.Any(x => x == player.PlayerInfo.PlayerId))
            {
                if (playerNames.AppSettings.Settings[player.PlayerInfo.PlayerId].Value != player.PlayerInfo.PlayerName)
                {
                    playerNames.AppSettings.Settings.Remove(player.PlayerInfo.PlayerId);
                    playerNames.AppSettings.Settings.Add(player.PlayerInfo.PlayerId, player.PlayerInfo.PlayerName);
                }
            }
            else
            {
                playerNames.AppSettings.Settings.Add(player.PlayerInfo.PlayerId, player.PlayerInfo.PlayerName);
            }

            playerNames.Save();

            Configuration playerRanks = HelpFunctions.LoadPlayerRanks();

            if (playerRanks.AppSettings.Settings.AllKeys!.Any(x => x == player.PlayerInfo.PlayerId))
            {
                if (int.Parse(playerRanks.AppSettings.Settings[player.PlayerInfo.PlayerId].Value!)
                    != player.PlayerInfo.Rank)
                {
                    playerRanks.AppSettings.Settings.Remove(player.PlayerInfo.PlayerId);
                    playerRanks.AppSettings.Settings.Add(player.PlayerInfo.PlayerId, player.PlayerInfo.Rank.ToString());
                }
            }
            else
            {
                playerRanks.AppSettings.Settings.Add(player.PlayerInfo.PlayerId, player.PlayerInfo.Rank.ToString());
            }

            playerRanks.Save();

            int weeklyChange = int.Parse(player.PlayerInfo.RankHistory.Split(",")[^7]) - player.PlayerInfo.Rank;
            string weeklyChangeText = "```diff\n- " + -weeklyChange + "```";

            if (weeklyChange >= 0)
            {
                weeklyChangeText = "```diff\n+ " + weeklyChange + "```";
            }

            EmbedBuilder embedBuilder = new ()
            {
                Title = player.PlayerInfo.PlayerName,
                ThumbnailUrl = "https://new.scoresaber.com" + player.PlayerInfo.Avatar,
                Url = "https://new.scoresaber.com/u/" + player.PlayerInfo.PlayerId,
            };

            embedBuilder.AddField("Player Stats", "\u200B");
            embedBuilder.AddField("Global Rank", player.PlayerInfo.Rank, true);

            embedBuilder.AddField(
                ":flag_" + player.PlayerInfo.Country.ToLower() + ": Rank",
                player.PlayerInfo.CountryRank,
                true);

            embedBuilder.AddField("PP", player.PlayerInfo.Pp, true);

            if (!string.IsNullOrEmpty(player.PlayerInfo.Role))
            {
                embedBuilder.AddField("Roles", player.PlayerInfo.Role, true);
            }

            embedBuilder.AddField("Weekly Change", weeklyChangeText, true);
            embedBuilder.AddField("Active Status", player.PlayerInfo.ActiveStatus, true);
            embedBuilder.AddField("Banned Status", player.PlayerInfo.BanStatus, true);

            embedBuilder.AddField(
                "------------------------------------------------------------------",
                "\u200B");

            embedBuilder.AddField("Score Information", "\u200B");
            embedBuilder.AddField("Total Score", player.ScoreStats.TotalScore, true);
            embedBuilder.AddField("Total Ranked Score", player.ScoreStats.TotalRankedScore, true);

            embedBuilder.AddField(
                "Avg Ranked Accuracy",
                (Math.Round(player.ScoreStats.AverageRankedAccuracy * 100) / 100) + " %");

            embedBuilder.AddField("Play Count", player.ScoreStats.TotalPlayCount, true);
            embedBuilder.AddField("Ranked Play Count", player.ScoreStats.RankedPlayCount, true);
            embedBuilder.WithFooter($"API Calls Left: {api.RateLimitRemaining}");

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the profile command without a player id specified.
        /// </summary>
        /// <returns> . </returns>
        [Command("profile")]
        public async Task MeProfileAsync()
        {
            Configuration players = HelpFunctions.LoadPlayers();

            bool exists = players.AppSettings.Settings.AllKeys!
                .Any(k => k == this.Context.Message.Author.Id.ToString());

            if (exists)
            {
                long id = long.Parse(players.AppSettings.Settings[this.Context.Message.Author.Id.ToString()].Value!);
                await this.ProfileAsync(id);
            }
            else
            {
                string prefix = HelpFunctions.LoadConfig().AppSettings.Settings["prefix"].Value;
                await this.Context.Channel.SendMessageAsync(
                    $"You don't have your score saber account linked to your discord profile. " +
                    $"Use \"{prefix}help link\"for more information");
            }
        }
    }
}