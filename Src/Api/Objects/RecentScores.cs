using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object of the RecentScores of the player.
    /// </summary>
    public class RecentScores
    {
        /// <summary>
        ///     Gets or sets the array of Scores from the player.
        /// </summary>
        [JsonPropertyName("scores")]
        public ScoreObject[] Scores { get; set; }
    }
}