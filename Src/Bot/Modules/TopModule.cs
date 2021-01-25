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
    ///     Module for the top command.
    /// </summary>
    public class TopModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the top command with a specified score saber id.
        /// </summary>
        /// <param name="playerId"> Score saber id of the player. </param>
        /// <returns> . </returns>
        [Command("top")]
        public async Task TopAsync(long playerId)
        {
            Api.Objects.Api api = Program.GetApi();

            TopScores topScores = await api.GetTopScoresOfPlayerAsync(playerId);
            ScoreObject score = topScores.Scores[0];

            if (string.IsNullOrEmpty(score.SongHash))
            {
                await this.Context.Channel.SendMessageAsync("This user doesn't have any top plays");

                return;
            }

            Configuration playerNames = HelpFunctions.LoadPlayerNames();

            bool exists = playerNames.AppSettings.Settings.AllKeys!.Any(k => k == playerId.ToString());

            string playerName;

            if (exists)
            {
                playerName = playerNames.AppSettings.Settings[playerId.ToString()].Value!;
            }
            else
            {
                Player player = await api.GetPlayerAsync(playerId);
                playerName = player.PlayerInfo.PlayerName;
                playerNames.AppSettings.Settings.Add(playerId.ToString(), playerName);
                playerNames.Save();
            }

            EmbedBuilder embedBuilder = new ()
            {
                Title = "Top Play of " + playerName,
                ThumbnailUrl = $"https://scoresaber.com/imports/images/songs/{score.SongHash}.png",
                Url = $"https://new.scoresaber.com/u/{playerId}",
            };

            string fullName = score.SongName + ", " + score.SongSubName;

            if (string.IsNullOrEmpty(score.SongSubName))
            {
                fullName = score.SongName;
            }

            embedBuilder.AddField("Map", fullName, true);
            embedBuilder.AddField("Artist", score.SongAuthorName, true);
            embedBuilder.AddField("Mapper", score.LevelAuthorName, true);
            embedBuilder.AddField("PP", score.Pp, true);
            embedBuilder.AddField("Weighted PP", Math.Round(score.Pp * score.Weight * 100) / 100, true);

            embedBuilder.AddField(
                "Accuracy",
                (Math.Round((double)score.Score / score.MaxScore * 10000) / 100) + " %",
                true);

            embedBuilder.AddField("Difficulty", score.DifficultyRaw, true);

            embedBuilder.AddField(
                "Map Rank",
                $"[{score.Rank}](https://scoresaber.com/leaderboard/{score.LeaderboardId})",
                true);

            string mods = score.Mods;

            if (string.IsNullOrEmpty(mods))
            {
                mods = "None";
            }

            embedBuilder.AddField("Mods", mods, true);
            embedBuilder.AddField("Score", score.Score + " / " + score.MaxScore, true);
            embedBuilder.AddField("Unmodified Score", score.UnmodifiedScore, true);

            embedBuilder.WithFooter("API Calls Left: " + api.RateLimitRemaining);

            await this.Context.Message.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        /// <summary>
        ///     Function that gets executed when someone uses the top command without a specified id.
        /// </summary>
        /// <returns> . </returns>
        [Command("top")]
        public async Task MeTopAsync()
        {
            Configuration players = HelpFunctions.LoadPlayers();

            bool exists = players.AppSettings.Settings.AllKeys!.Any(k => k == this.Context.Message.Author.Id.ToString());

            if (exists)
            {
                long id = long.Parse(players.AppSettings.Settings[this.Context.Message.Author.Id.ToString()].Value!);
                await this.TopAsync(id);
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