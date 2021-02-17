using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Bot.Api.Objects;
using Bot.Bot.FileObjects;
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
        /// <returns> An empty task. </returns>
        [Command("profile")]
        public async Task ProfileAsync(long playerId)
        {
            Api.Objects.Api api = Program.GetApi();

            Player player = await api.GetPlayerAsync(playerId);

            List<PlayerInformation> playerInformationList = PlayerInformation.FromJson();

            if (playerInformationList!.Any(playerInfo => playerInfo.Id == long.Parse(player.PlayerInfo.PlayerId!)))
            {
                PlayerInformation playerInformation =
                    playerInformationList.Find(playerInfo => playerInfo.Id == long.Parse(player.PlayerInfo.PlayerId!));

                playerInformation.Name = player.PlayerInfo.PlayerName;
                playerInformation.Rank = player.PlayerInfo.Rank;
            }
            else
            {
                playerInformationList.Add(new PlayerInformation
                {
                    Id = playerId,
                    Name = player.PlayerInfo.PlayerName,
                    Rank = player.PlayerInfo.Rank,
                });
            }

            await File.WriteAllTextAsync("playerinformation.json", JsonSerializer.Serialize(playerInformationList));

            NumberFormatInfo numberFormatInfo = new () { NumberGroupSeparator = "," };

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
                Color = Color.Blue,
            };

            embedBuilder.AddField("Player Stats", "\u200B");
            embedBuilder.AddField("Global Rank", player.PlayerInfo.Rank.ToString("#,#", numberFormatInfo), true);

            embedBuilder.AddField(
                ":flag_" + player.PlayerInfo.Country.ToLower() + ": Rank",
                player.PlayerInfo.CountryRank.ToString("#,#", numberFormatInfo),
                true);

            embedBuilder.AddField("PP", player.PlayerInfo.Pp.ToString("#,#.##", numberFormatInfo), true);

            if (!string.IsNullOrEmpty(player.PlayerInfo.Role))
            {
                embedBuilder.AddField("Roles", player.PlayerInfo.Role, true);
            }

            embedBuilder.AddField("Weekly Change", weeklyChangeText, true);
            embedBuilder.AddField("Active Status", player.PlayerInfo.ActiveStatus, true);
            embedBuilder.AddField("Banned Status", player.PlayerInfo.BanStatus, true);
            embedBuilder.AddField("------------------------------------------------------------------", "\u200B");
            embedBuilder.AddField("Score Information", "\u200B");
            embedBuilder.AddField("Total Score", player.ScoreStats.TotalScore.ToString("#,#", numberFormatInfo), true);

            embedBuilder.AddField(
                "Total Ranked Score",
                player.ScoreStats.TotalRankedScore.ToString("#,#", numberFormatInfo),
                true);

            embedBuilder.AddField("Avg Ranked Accuracy", (Math.Round(player.ScoreStats.AverageRankedAccuracy * 100) / 100) + " %");
            embedBuilder.AddField("Play Count", player.ScoreStats.TotalPlayCount.ToString("#,#", numberFormatInfo), true);
            embedBuilder.AddField("Ranked Play Count", player.ScoreStats.RankedPlayCount.ToString("#,#", numberFormatInfo), true);

            embedBuilder.WithFooter($"API Calls Left: {api.RateLimitRemaining}");

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the profile command without a player id specified.
        /// </summary>
        /// <returns> An empty task. </returns>
        [Command("profile")]
        public async Task MeProfileAsync()
        {
            List<LinkedPlayer> linkedPlayers = LinkedPlayer.FromJson();

            bool exists = linkedPlayers!.Any(player => player.DiscordId == this.Context.Message.Author.Id);

            if (exists)
            {
                long id = linkedPlayers.Find(player => player.DiscordId == this.Context.Message.Author.Id).ScoreSaberId;

                await this.ProfileAsync(id);
            }
            else
            {
                string prefix = Config.Default.Prefix;

                await this.Context.Channel.SendMessageAsync(
                    $"You don't have your score saber account linked to your discord profile. Use \"{prefix}help link\" for more information " +
                    $"or use \"{prefix}profile [scoresaberid]\" instead");
            }
        }
    }
}