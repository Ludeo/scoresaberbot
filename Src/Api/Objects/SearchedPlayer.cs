using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object of a player that got searched.
    /// </summary>
    public class SearchedPlayer
    {
        /// <summary>
        ///     Gets or sets the Id of a player.
        /// </summary>
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        /// <summary>
        ///     Gets or sets the name of a player.
        /// </summary>
        [JsonPropertyName("playerName")]
        public string PlayerName { get; set; }

        /// <summary>
        ///     Gets or sets the rank of a player.
        /// </summary>
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        /// <summary>
        ///     Gets or sets the ammount of PP of a player.
        /// </summary>
        [JsonPropertyName("pp")]
        public double Pp { get; set; }

        /// <summary>
        ///     Gets or sets the avatar link of a player.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        ///     Gets or sets the country of a player.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets the history of the rank of a player.
        /// </summary>
        [JsonPropertyName("history")]
        public string RankHistory { get; set; }

        /// <summary>
        ///     Gets or sets the difference of ranks from the player compared to a week ago.
        /// </summary>
        [JsonPropertyName("difference")]
        public int RankDifference { get; set; }
    }
}