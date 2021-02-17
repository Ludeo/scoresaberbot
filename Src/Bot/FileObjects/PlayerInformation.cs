using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Bot.FileObjects
{
    /// <summary>
    ///     Class that represents the player information file.
    /// </summary>
    public class PlayerInformation
    {
        /// <summary>
        ///     Gets or sets the score saber id of a player.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets the name of a player.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the rank of a player.
        /// </summary>
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        /// <summary>
        ///     Reads the player information file and returns it.
        /// </summary>
        /// <returns> A List of PlayerInformation. </returns>
        public static List<PlayerInformation> FromJson()
        {
            if (!File.Exists("playerinformation.json"))
            {
                List<PlayerInformation> playerInformation = new ();

                File.WriteAllText("playerinformation.json", JsonSerializer.Serialize(playerInformation));
            }

            return JsonSerializer.Deserialize<List<PlayerInformation>>(
                File.ReadAllText("playerinformation.json"));
        }
    }
}