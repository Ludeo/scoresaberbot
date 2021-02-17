using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Bot.Api.Objects;
using Bot.Bot.FileObjects;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the addtrack command.
    /// </summary>
    public class AddTrackModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when the admin uses the addtrack command.
        /// </summary>
        /// <param name="playerId"> The score saber id of the player that should get added to tracking. </param>
        /// <returns> An empty task. </returns>
        [Command("addtrack")]
        public async Task AddTrackAsync(long playerId)
        {
            Config config = Config.Default;

            if (this.Context.Message.Author.Id != config.AdminId)
            {
                return;
            }

            List<TrackedPlayer> trackedPlayers = TrackedPlayer.FromJson();

            if (trackedPlayers!.Any(player => player.Id == playerId))
            {
                await this.Context.Channel.SendMessageAsync("This player is already getting tracked");

                return;
            }

            Api.Objects.Api api = Program.GetApi();

            List<PlayerInformation> playerInformation = PlayerInformation.FromJson();

            if (playerInformation!.All(playerInfo => playerInfo.Id != playerId))
            {
                Player player = await api.GetPlayerAsync(playerId);

                playerInformation.Add(
                    new PlayerInformation
                    {
                        Id = playerId,
                        Name = player.PlayerInfo.PlayerName,
                        Rank = player.PlayerInfo.Rank,
                    });

                await File.WriteAllTextAsync(
                    "playerinformation.json", JsonSerializer.Serialize(playerInformation));
            }

            DateTime timeNow = DateTime.Now;

            string timeNowConverted = timeNow.ToString("yyyy-MM-ddThh:mm:ss.fffZ");

            trackedPlayers.Add(new TrackedPlayer { Id = playerId, LastScore = timeNowConverted });

            await File.WriteAllTextAsync("trackedplayers.json", JsonSerializer.Serialize(trackedPlayers));

            await this.Context.Channel.SendMessageAsync(
                "The player with the id " + playerId + " is now getting tracked");
        }
    }
}