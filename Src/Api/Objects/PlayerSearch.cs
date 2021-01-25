using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object of SearchedPlayers that you get when searching for a player by name.
    /// </summary>
    public class PlayerSearch
    {
        /// <summary>
        ///     Gets or sets an array of players after you searched for a name.
        /// </summary>
        [JsonPropertyName("players")]
        public SearchedPlayer[] Players { get; set; }
    }
}