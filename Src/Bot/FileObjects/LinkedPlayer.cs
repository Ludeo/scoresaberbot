using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Bot.FileObjects
{
    /// <summary>
    ///     Class that represents the linked player file.
    /// </summary>
    public class LinkedPlayer
    {
        /// <summary>
        ///     Gets or sets the discord id of the linked player.
        /// </summary>
        [JsonPropertyName("discordid")]
        public ulong DiscordId { get; set; }

        /// <summary>
        ///     Gets or sets the score saber id of the linked player.
        /// </summary>
        [JsonPropertyName("scoresaberid")]
        public long ScoreSaberId { get; set; }

        /// <summary>
        ///     Reads the linked player file and returns it.
        /// </summary>
        /// <returns> A List of LinkedPlayer. </returns>
        public static List<LinkedPlayer> FromJson()
        {
            if (!File.Exists("linkedplayers.json"))
            {
                List<LinkedPlayer> linkedPlayers = new ();

                File.WriteAllText("linkedplayers.json", JsonSerializer.Serialize(linkedPlayers));
            }

            return JsonSerializer.Deserialize<List<LinkedPlayer>>(File.ReadAllText("linkedplayers.json"));
        }
    }
}