using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object for a Badge of a player.
    /// </summary>
    public class Badge
    {
        /// <summary>
        ///     Gets or Sets the Image of the Badge.
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        ///     Gets or Sets the Description of the Badge.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}