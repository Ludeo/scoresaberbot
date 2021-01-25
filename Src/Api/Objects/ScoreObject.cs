using System.Text.Json.Serialization;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object for a score on a map.
    /// </summary>
    public class ScoreObject
    {
        /// <summary>
        ///     Gets or sets the Rank from the player on the map.
        /// </summary>
        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        /// <summary>
        ///     Gets or sets the Id of the score.
        /// </summary>
        [JsonPropertyName("scoreId")]
        public int ScoreId { get; set; }

        /// <summary>
        ///     Gets or sets the score the player got on the map.
        /// </summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>
        ///     Gets or sets the unmodified score the player got on the map.
        /// </summary>
        [JsonPropertyName("unmodififiedScore")]
        public int UnmodifiedScore { get; set; }

        /// <summary>
        ///     Gets or sets the mods the player used on the map.
        /// </summary>
        [JsonPropertyName("mods")]
        public string Mods { get; set; }

        /// <summary>
        ///     Gets or sets the ammount of pp the player got on the map.
        /// </summary>
        [JsonPropertyName("pp")]
        public double Pp { get; set; }

        /// <summary>
        ///     Gets or sets the weight of the score that it has on the rank of the player.
        /// </summary>
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        ///     Gets or sets the time when this score was made.
        /// </summary>
        [JsonPropertyName("timeSet")]
        public string TimeSet { get; set; }

        /// <summary>
        ///     Gets or sets the id of the leaderboard of the map.
        /// </summary>
        [JsonPropertyName("leaderboardId")]
        public int LeaderboardId { get; set; }

        /// <summary>
        ///     Gets or sets the hash of the map.
        /// </summary>
        [JsonPropertyName("songHash")]
        public string SongHash { get; set; }

        /// <summary>
        ///     Gets or sets the name of the map.
        /// </summary>
        [JsonPropertyName("songName")]
        public string SongName { get; set; }

        /// <summary>
        ///     Gets or sets the subname of the map.
        /// </summary>
        [JsonPropertyName("songSubName")]
        public string SongSubName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author from the song of the map.
        /// </summary>
        [JsonPropertyName("songAuthorName")]
        public string SongAuthorName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the author from the map.
        /// </summary>
        [JsonPropertyName("levelAuthorName")]
        public string LevelAuthorName { get; set; }

        /// <summary>
        ///     Gets or sets the difficulty of the map.
        /// </summary>
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; }

        /// <summary>
        ///     Gets or sets the raw difficulty of the map.
        /// </summary>
        [JsonPropertyName("difficultyRaw")]
        public string DifficultyRaw { get; set; }

        /// <summary>
        ///     Gets or sets the maximum score that someone can get on the map.
        /// </summary>
        [JsonPropertyName("maxScore")]
        public int MaxScore { get; set; }
    }
}