using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Bot.Api.Objects;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the updatename command.
    /// </summary>
    public class UpdateNameModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the updatename command with a player id specified.
        /// </summary>
        /// <param name="playerId"> The score saber id of the player which user name should get updated. </param>
        /// <returns> . </returns>
        [Command("updatename")]
        public async Task UpdateNameAsync(long playerId)
        {
            Api.Objects.Api api = Program.GetApi();
            Configuration playerNames = HelpFunctions.LoadPlayerNames();

            bool exists = playerNames.AppSettings.Settings.AllKeys!.Any(k => k == playerId.ToString());

            Player player = await api.GetPlayerAsync(playerId);

            if (exists)
            {
                playerNames.AppSettings.Settings.Remove(playerId.ToString());
                playerNames.AppSettings.Settings.Add(playerId.ToString(), player.PlayerInfo.PlayerName);
            }
            else
            {
                playerNames.AppSettings.Settings.Add(playerId.ToString(), player.PlayerInfo.PlayerName);
            }

            playerNames.Save();

            await this.Context.Channel.SendMessageAsync("Successfully updated the username of " + playerId);
        }

        /// <summary>
        ///     Function that gets executed when someone uses the updatename command without a player id specified.
        /// </summary>
        /// <returns> . </returns>
        [Command("updatename")]
        public async Task MeUpdateNameAsync()
        {
            Configuration players = HelpFunctions.LoadPlayers();

            bool exists = players.AppSettings.Settings.AllKeys!.Any(k => k == this.Context.Message.Author.Id.ToString());

            if (exists)
            {
                long id = long.Parse(players.AppSettings.Settings[this.Context.Message.Author.Id.ToString()].Value!);
                await this.UpdateNameAsync(id);
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