using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object for a player from Score Saber.
    /// </summary>
    public class Player
    {
        /// <summary>
        ///     Gets or Sets the PlayerInfo of a player.
        /// </summary>
        [JsonPropertyName("playerInfo")]
        public PlayerInfo PlayerInfo { get; set; }

        /// <summary>
        ///     Gets or Sets the ScoreStats of a player.
        /// </summary>
        [JsonPropertyName("scoreStats")]
        public ScoreStats ScoreStats { get; set; }
    }
}