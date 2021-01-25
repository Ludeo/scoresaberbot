﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Bot.Api.Objects;
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
        /// <returns> . </returns>
        [Command("starttrack")]
        public async Task StartTrackAsync()
        {
            Configuration config = HelpFunctions.LoadConfig();

            if (this.Context.Message.Author.Id.ToString() != config.AppSettings.Settings["adminid"].Value)
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
            Configuration trackedPlayers = HelpFunctions.LoadTrackedPlayers();

            foreach (string playerId in trackedPlayers.AppSettings.Settings.AllKeys)
            {
                RecentScores recentScores = api.GetRecentScoresOfPlayerAsync(long.Parse(playerId!)).Result;
                ScoreObject score = recentScores.Scores[0];

                DateTime recentTime = DateTime.Parse(score.TimeSet!);
                DateTime oldTime = DateTime.Parse(trackedPlayers.AppSettings.Settings[playerId].Value!);

                if (oldTime.CompareTo(recentTime) == -1)
                {
                    trackedPlayers.AppSettings.Settings[playerId].Value = score.TimeSet;
                    trackedPlayers.Save();

                    Configuration playerNames = HelpFunctions.LoadPlayerNames();

                    bool exists = playerNames.AppSettings.Settings.AllKeys!.Any(k => k == playerId);

                    string playerName;

                    if (exists)
                    {
                        playerName = playerNames.AppSettings.Settings[playerId].Value!;
                    }
                    else
                    {
                        Player player = api.GetPlayerAsync(long.Parse(playerId)).Result;
                        playerName = player.PlayerInfo.PlayerName;
                        playerNames.AppSettings.Settings.Add(playerId, playerName);
                        playerNames.Save();
                    }

                    EmbedBuilder embedBuilder = new ()
                    {
                        Title = "New Score from " + playerName,
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

                    DiscordSocketClient client = Program.Client;

                    Configuration config = HelpFunctions.LoadConfig();

                    client.GetGuild(ulong.Parse(config.AppSettings.Settings["trackserver"].Value))
                          .GetTextChannel(ulong.Parse(config.AppSettings.Settings["trackchannel"].Value))
                                               .SendMessageAsync(string.Empty, false, embedBuilder.Build());
                }
            }
        }
    }
}