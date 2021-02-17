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
    ///     Module for the recent command.
    /// </summary>
    public class RecentModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the recent command with a specified score saber id.
        /// </summary>
        /// <param name="playerId"> Score saber id of the player. </param>
        /// <returns> An empty task. </returns>
        [Command("recent")]
        public async Task RecentAsync(long playerId)
        {
            Api.Objects.Api api = Program.GetApi();

            RecentScores recentScores = await api.GetRecentScoresOfPlayerAsync(playerId);
            ScoreObject score = recentScores.Scores[0];

            if (string.IsNullOrEmpty(score.SongHash))
            {
                await this.Context.Channel.SendMessageAsync("This user doesn't have any recent plays");

                return;
            }

            List<PlayerInformation> playerInformationList = PlayerInformation.FromJson();

            bool exists = playerInformationList!.Any(playerInformation => playerInformation.Id == playerId);

            string playerName;

            if (exists)
            {
                playerName = playerInformationList.Find(playerInformation => playerInformation.Id == playerId).Name;
            }
            else
            {
                Player player = await api.GetPlayerAsync(playerId);
                playerName = player.PlayerInfo.PlayerName;
                playerInformationList.Add(new PlayerInformation
                {
                    Id = long.Parse(player.PlayerInfo.PlayerId!),
                    Name = playerName,
                    Rank = player.PlayerInfo.Rank,
                });

                await File.WriteAllTextAsync("playerinformation.json", JsonSerializer.Serialize(playerInformationList));
            }

            EmbedBuilder embedBuilder = new ()
            {
                Title = "Most recent Play of " + playerName,
                ThumbnailUrl = $"https://scoresaber.com/imports/images/songs/{score.SongHash}.png",
                Url = $"https://new.scoresaber.com/u/{playerId}",
                Color = Color.Orange,
            };

            string fullName = score.SongName + ", " + score.SongSubName;

            if (string.IsNullOrEmpty(score.SongSubName))
            {
                fullName = score.SongName;
            }

            embedBuilder.AddField("Map", fullName, true);
            embedBuilder.AddField("Artist", score.SongAuthorName, true);
            embedBuilder.AddField("Mapper", score.LevelAuthorName, true);

            NumberFormatInfo numberFormatInfo = new () { NumberGroupSeparator = "," };

            embedBuilder.AddField("PP", score.Pp.ToString("#,#.##", numberFormatInfo), true);

            embedBuilder.AddField(
                "Weighted PP",
                (Math.Round(score.Pp * score.Weight * 100) / 100).ToString("#,#.##", numberFormatInfo),
                true);

            embedBuilder.AddField("Accuracy", (Math.Round((double)score.Score / score.MaxScore * 10000) / 100) + " %", true);
            embedBuilder.AddField("Difficulty", score.DifficultyRaw, true);

            embedBuilder.AddField(
                "Map Rank",
                $"[{score.Rank.ToString("#,#", numberFormatInfo)}](https://scoresaber.com/leaderboard/{score.LeaderboardId})",
                true);

            string mods = score.Mods;

            if (string.IsNullOrEmpty(mods))
            {
                mods = "None";
            }

            embedBuilder.AddField("Mods", mods, true);

            embedBuilder.AddField(
                "Score",
                score.Score.ToString("#,#", numberFormatInfo) + " / " + score.MaxScore.ToString("#,#", numberFormatInfo),
                true);

            embedBuilder.AddField("Unmodified Score", score.UnmodifiedScore.ToString("#,#", numberFormatInfo), true);

            embedBuilder.WithFooter("API Calls Left: " + api.RateLimitRemaining);

            await this.Context.Message.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the recent command without a specified id.
        /// </summary>
        /// <returns> An empty task. </returns>
        [Command("recent")]
        public async Task MeRecentAsync()
        {
            List<LinkedPlayer> linkedPlayers = LinkedPlayer.FromJson();

            bool exists = linkedPlayers!.Any(player => player.DiscordId == this.Context.Message.Author.Id);

            if (exists)
            {
                long id = linkedPlayers.Find(player => player.DiscordId == this.Context.Message.Author.Id).ScoreSaberId;

                await this.RecentAsync(id);
            }
            else
            {
                string prefix = Config.Default.Prefix;

                await this.Context.Channel.SendMessageAsync(
                    $"You don't have your score saber account linked to your discord profile. Use \"{prefix}help link\" for more information " +
                    $"or use \"{prefix}recent [scoresaberid]\" instead");
            }
        }
    }
}