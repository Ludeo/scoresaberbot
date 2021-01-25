using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object of SearchedPlayers when you access a page on the leaderboard.
    /// </summary>
    public class PlayerRanking
    {
        /// <summary>
        ///     Gets or sets an array of players from the leaderboard page
        /// </summary>
        [JsonPropertyName("players")]
        public SearchedPlayer[] Players { get; set; }
    }
}