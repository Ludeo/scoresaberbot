using System.Text.Json.Serialization;
using Bot.Api.Enums;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object representing the PlayerInfo of a player.
    /// </summary>
    public class PlayerInfo
    {
        /// <summary>
        ///     Gets or sets the PlayerId of a player.
        /// </summary>
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        /// <summary>
        ///     Gets or sets the PlayerName of the player.
        /// </summary>
        [JsonPropertyName("playerName")]
        public string PlayerName { get; set; }

        /// <summary>
        ///     Gets or sets the Url to the Avatar of a player.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        ///     Gets or sets the Rank of a player.
        /// </summary>
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        /// <summary>
        ///     Gets or sets the CountryRank of a player.
        /// </summary>
        [JsonPropertyName("countryRank")]
        public int CountryRank { get; set; }

        /// <summary>
        ///     Gets or sets the amount of PP from a player.
        /// </summary>
        [JsonPropertyName("pp")]
        public double Pp { get; set; }

        /// <summary>
        ///     Gets or sets the Country of a player.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        ///     Gets or sets the Role of a player.
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; }

        /// <summary>
        ///     Gets or sets the Badges of a player.
        /// </summary>
        [JsonPropertyName("badges")]
        public Badge[] Badges { get; set; }

        /// <summary>
        ///     Gets or sets the RankHistory of a player.
        /// </summary>
        [JsonPropertyName("history")]
        public string RankHistory { get; set; }

        /// <summary>
        ///     Gets or sets the Permissions of a player.
        /// </summary>
        [JsonPropertyName("permissions")]
        public int Permissions { get; set; }

        /// <summary>
        ///     Gets or sets the ActiveStatus of a player.
        /// </summary>
        [JsonPropertyName("inactive")]
        public ActiveStatus ActiveStatus { get; set; }

        /// <summary>
        ///     Gets or sets the BanStatus of a player.
        /// </summary>
        [JsonPropertyName("banned")]
        public BanStatus BanStatus { get; set; }
    }
}