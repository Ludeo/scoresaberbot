namespace Bot.Bot.Objects
{
    /// <summary>
    ///     Object for a player for the discord leaderboard.
    /// </summary>
    public class LeaderboardPlayer
    {
        /// <summary>
        ///     Gets or sets the discord id of the player.
        /// </summary>
        public ulong DiscordId { get; set; }

        /// <summary>
        ///     Gets or sets the score saber id of the player.
        /// </summary>
        public long ScoresaberId { get; set; }

        /// <summary>
        ///     Gets or sets the score saber rank of the player.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        ///     Gets or sets the score saber name of the player.
        /// </summary>
        public string Name { get; set; }
    }
}