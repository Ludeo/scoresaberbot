using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object for the ScoreStats of a player.
    /// </summary>
    public class ScoreStats
    {
        /// <summary>
        ///     Gets or sets the TotalScore of a player.
        /// </summary>
        [JsonPropertyName("totalScore")]
        public long TotalScore { get; set; }

        /// <summary>
        ///     Gets or sets the TotalScore of a player that where achieved on RANKED maps.
        /// </summary>
        [JsonPropertyName("totalRankedScore")]
        public long TotalRankedScore { get; set; }

        /// <summary>
        ///     Gets or sets the AverageAccuracy on RANKED maps.
        /// </summary>
        [JsonPropertyName("averageRankedAccuracy")]
        public double AverageRankedAccuracy { get; set; }

        /// <summary>
        ///     Gets or sets the total play count of a player.
        /// </summary>
        [JsonPropertyName("totalPlayCount")]
        public int TotalPlayCount { get; set; }

        /// <summary>
        ///     Gets or sets the total play count of a player on RANKED maps.
        /// </summary>
        [JsonPropertyName("rankedPlayCount")]
        public int RankedPlayCount { get; set; }
    }
}