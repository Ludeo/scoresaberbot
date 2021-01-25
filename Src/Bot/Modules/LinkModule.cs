using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the link command.
    /// </summary>
    public class LinkModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Links a discord account to a score saber account.
        /// </summary>
        /// <param name="playerId"> The score saber id the discord account should get linked to. </param>
        /// <returns> . </returns>
        [Command("link")]
        public async Task LinkAsync(long playerId)
        {
            Configuration players = HelpFunctions.LoadPlayers();

            bool exists = players.AppSettings.Settings.AllKeys!.Any(k => k == this.Context.Message.Author.Id.ToString());

            if (exists)
            {
                players.AppSettings.Settings.Remove(this.Context.Message.Author.Id.ToString());
                players.AppSettings.Settings.Add(this.Context.Message.Author.Id.ToString(), playerId.ToString());
                players.Save();

                await this.Context.Channel.SendMessageAsync("Successfully updated your linked score saber id");

                return;
            }

            players.AppSettings.Settings.Add("311861142114926593", "76561198143629166");
            players.Save();

            await this.Context.Channel.SendMessageAsync("Successfully linked your score saber id.");
        }
    }
}