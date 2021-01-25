using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
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
        /// <returns> . </returns>
        [Command("deletetrack")]
        public async Task DeleteTrackAsync(long playerId)
        {
            Configuration config = HelpFunctions.LoadConfig();

            if (this.Context.Message.Author.Id != ulong.Parse(config.AppSettings.Settings["adminid"].Value!))
            {
                return;
            }

            Configuration trackedPlayers = HelpFunctions.LoadTrackedPlayers();

            if (trackedPlayers.AppSettings.Settings.AllKeys!.Any(id => id == playerId.ToString()))
            {
                trackedPlayers.AppSettings.Settings.Remove(playerId.ToString());
                trackedPlayers.Save();

                await this.Context.Channel.SendMessageAsync("Player was removed from tracking.");

                return;
            }

            await this.Context.Channel.SendMessageAsync("This player was not getting tracked.");
        }
    }
}