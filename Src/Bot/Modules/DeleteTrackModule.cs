using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Bot.Bot.FileObjects;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the deletetrack command.
    /// </summary>
    public class DeleteTrackModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed if the admin runs the deletetrack command.
        /// </summary>
        /// <param name="playerId"> Score saber id of the player that should get removed from tracking. </param>
        /// <returns> An empty task. </returns>
        [Command("deletetrack")]
        public async Task DeleteTrackAsync(long playerId)
        {
            Config config = Config.Default;

            if (this.Context.Message.Author.Id != config.AdminId)
            {
                return;
            }

            List<TrackedPlayer> trackedPlayers = TrackedPlayer.FromJson();

            if (trackedPlayers!.Any(player => player.Id == playerId))
            {
                trackedPlayers.Remove(trackedPlayers.Find(player => player.Id == playerId));

                await File.WriteAllTextAsync(
                    "trackedplayers.json", JsonSerializer.Serialize(trackedPlayers));

                await this.Context.Channel.SendMessageAsync("Player was removed from tracking.");

                return;
            }

            await this.Context.Channel.SendMessageAsync("This player was not getting tracked.");
        }
    }
}