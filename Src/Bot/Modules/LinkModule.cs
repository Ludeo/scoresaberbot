using System;
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
            List<LinkedPlayer> linkedPlayers = LinkedPlayer.FromJson();

            ulong id = this.Context.Message.Author.Id;

            bool exists = linkedPlayers!.Any(player => player.DiscordId == id);

            if (exists)
            {
                linkedPlayers.Remove(linkedPlayers.Find(player => player.DiscordId == id));
            }

            linkedPlayers.Add(new LinkedPlayer { ScoreSaberId = playerId, DiscordId = id });

            await File.WriteAllTextAsync("linkedplayers.json", JsonSerializer.Serialize(linkedPlayers));

            await this.Context.Channel.SendMessageAsync(
                $"Successfully {(exists ? "updated" : "added")} your linked score saber id");
        }

        /// <inheritdoc cref="LinkAsync(long)"/>
        [Command("link")]
        public async Task LinkAsync(string link)
        {
            Uri uri = new (link!);
            string idSegment = uri.Segments[2];

            if (uri.Host != "new.scoresaber.com" || uri.Host != "scoresaber.com")
            {
                await this.Context.Channel.SendMessageAsync("Please only post links to the scoresaber website.");
            }
            else if (!long.TryParse(idSegment, out long playerId))
            {
                await this.Context.Channel.SendMessageAsync(
                    "Incorrect link, please make sure you are posting the correct link.");
            }
            else
            {
                await this.LinkAsync(playerId);
            }
        }
    }
}