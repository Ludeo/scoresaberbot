using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Bot.FileObjects
{
    /// <summary>
    ///     Class that represents the tracked player file.
    /// </summary>
    public class TrackedPlayer
    {
        /// <summary>
        ///     Gets or sets the score saber id of a tracked player.
        /// </summary>
        [JsonPropertyName("scoresaberid")]
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets the date of the last score of a tracked player.
        /// </summary>
        [JsonPropertyName("lastscore")]
        public string LastScore { get; set; }

        /// <summary>
        ///     Reads the tracked players file and returns it.
        /// </summary>
        /// <returns> A List of TrackedPlayers. </returns>
        public static List<TrackedPlayer> FromJson()
        {
            if (!File.Exists("trackedplayers.json"))
            {
                List<TrackedPlayer> trackedPlayers = new ();

                File.WriteAllText("trackedplayers.json", JsonSerializer.Serialize(trackedPlayers));
            }

            return JsonSerializer.Deserialize<List<TrackedPlayer>>(File.ReadAllText("trackedplayers.json"));
        }
    }
}