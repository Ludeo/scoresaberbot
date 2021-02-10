using System;
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
        /// <returns> An empty task. </returns>
        [Command("link")]
        public async Task LinkAsync(long playerId)
        {
            Configuration players = HelpFunctions.LoadPlayers();

            string id = this.Context.Message.Author.Id.ToString();

            KeyValueConfigurationCollection settings = players.AppSettings.Settings;
            bool exists = settings.AllKeys!.Any(k => k == id);

            if (exists)
            {
                settings.Remove(id);
            }

            settings.Add(id, playerId.ToString());
            players.Save();

            await this.Context.Channel.SendMessageAsync($"Successfully {(exists ? "updated" : "added")} your linked score saber id");
        }

        /// <inheritdoc cref="LinkAsync(long)"/>
        [Command("link")]
        public async Task LinkAsync(string link)
        {
            Uri uri = new (link!);
            string idSegment = uri.Segments[2];

            if (!long.TryParse(idSegment, out long playerId))
            {
                await this.Context.Channel.SendMessageAsync($"Incorrect link, please use the scoresaber id or link.");
            }
            else
            {
                await this.LinkAsync(playerId);
            }
        }
    }
}