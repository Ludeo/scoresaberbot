using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Bot.Api.Objects;
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
            Configuration config = HelpFunctions.LoadConfig();

            if (this.Context.Message.Author.Id.ToString() != config.AppSettings.Settings["adminid"].Value)
            {
                return;
            }

            Configuration trackedPlayers = HelpFunctions.LoadTrackedPlayers();

            if (trackedPlayers.AppSettings.Settings.AllKeys!.Any(id => id == playerId.ToString()))
            {
                await this.Context.Channel.SendMessageAsync("This player is already getting tracked");

                return;
            }

            Api.Objects.Api api = Program.GetApi();

            Configuration playerNames = HelpFunctions.LoadPlayerNames();

            if (playerNames.AppSettings.Settings.AllKeys!.All(id => id != playerId.ToString()))
            {
                Player player = await api.GetPlayerAsync(playerId);

                playerNames.AppSettings.Settings.Add(playerId.ToString(), player.PlayerInfo.PlayerName);
                playerNames.Save();
            }

            DateTime timeNow = DateTime.Now;

            string timeNowConverted = timeNow.ToString("yyyy-MM-ddThh:mm:ss.fffZ");

            trackedPlayers.AppSettings.Settings.Add(playerId.ToString(), timeNowConverted);

            trackedPlayers.Save();

            await this.Context.Channel.SendMessageAsync(
                "The player with the id " + playerId + " is now getting tracked");
        }
    }
}