using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Bot.Api.Objects;
using Bot.Bot.FileObjects;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the starttrack command.
    /// </summary>
    public class StartTrackModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when the admin uses the starttrack command.
        /// </summary>
        /// <returns> An empty task. </returns>
        [Command("starttrack")]
        public async Task StartTrackAsync()
        {
            Config config = Config.Default;

            if (this.Context.Message.Author.Id != config.AdminId)
            {
                return;
            }

            Timer timer = Program.Timer;

            if (timer.Enabled)
            {
                await this.Context.Channel.SendMessageAsync("Tracking already started");

                return;
            }

            timer.Interval = 120000;
            timer.Enabled = true;
            timer.Elapsed += this.OnTimedEvent;

            await this.Context.Channel.SendMessageAsync("Tracking started");
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Api.Objects.Api api = Program.GetApi();
            List<TrackedPlayer> trackedPlayers = TrackedPlayer.FromJson();
            List<PlayerInformation> playerInformationList = PlayerInformation.FromJson();

            foreach (TrackedPlayer trackedPlayer in trackedPlayers)
            {
                RecentScores recentScores = api.GetRecentScoresOfPlayerAsync(trackedPlayer.Id).Result;
                ScoreObject score = recentScores.Scores[0];

                DateTime recentTime = DateTime.Parse(score.TimeSet!);
                DateTime oldTime = DateTime.Parse(trackedPlayer.LastScore!);

                long playerId = trackedPlayer.Id;

                if (oldTime.CompareTo(recentTime) == -1)
                {
                    trackedPlayer.LastScore = score.TimeSet;

                    bool exists = playerInformationList!.Any(playerInformation => playerInformation.Id == playerId);

                    string playerName;

                    if (exists)
                    {
                        playerName = playerInformationList.Find(playerInformation => playerInformation.Id == playerId).Name;
                    }
                    else
                    {
                        Player player = api.GetPlayerAsync(playerId).Result;
                        playerName = player.PlayerInfo.PlayerName;
                        playerInformationList.Add(new PlayerInformation
                        {
                            Id = playerId,
                            Name = player.PlayerInfo.PlayerName,
                            Rank = player.PlayerInfo.Rank,
                        });
                    }

                    EmbedBuilder embedBuilder = new ()
                    {
                        Title = "New Score from " + playerName,
                        ThumbnailUrl = $"https://scoresaber.com/imports/images/songs/{score.SongHash}.png",
                        Url = $"https://new.scoresaber.com/u/{playerId}",
                        Color = Color.Red,
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

                    double weightedPp = Math.Round(score.Pp * score.Weight * 100) / 100;

                    embedBuilder.AddField(
                        "Weighted PP",
                        weightedPp == 0 ? "0" : (Math.Round(score.Pp * score.Weight * 100) / 100).ToString("#,#.##", numberFormatInfo),
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

                    DiscordSocketClient client = Program.Client;

                    Config config = Config.Default;

                    client.GetGuild(config.TrackServer).GetTextChannel(config.TrackChannel)
                                               .SendMessageAsync(string.Empty, false, embedBuilder.Build());
                }
            }

            File.WriteAllText("playerinformation.json", JsonSerializer.Serialize(playerInformationList));

            File.WriteAllText("trackedplayers.json", JsonSerializer.Serialize(trackedPlayers));
        }
    }
}