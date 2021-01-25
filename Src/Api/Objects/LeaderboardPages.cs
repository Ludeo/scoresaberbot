using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object that represents how many pages of ranked players exist (one page is 50 player).
    /// </summary>
    public class LeaderboardPages
    {
        /// <summary>
        ///     Gets or sets the ammount of pages.
        /// </summary>
        [JsonPropertyName("pages")]
        public int Pages { get; set; }
    }
}