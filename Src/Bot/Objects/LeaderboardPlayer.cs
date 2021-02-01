namespace Bot.Bot.Objects
{
    /// <summary>
    ///     Object for a player for the discord leaderboard.
    /// </summary>
    public class LeaderboardPlayer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LeaderboardPlayer"/> class.
        /// </summary>
        /// <param name="discordId"> Discord Id of the player. </param>
        /// <param name="scoresaberId"> Score Saber Id of the player. </param>
        public LeaderboardPlayer(ulong discordId, long scoresaberId)
        {
            this.DiscordId = discordId;
            this.ScoresaberId = scoresaberId;
        }

        /// <summary>
        ///     Gets the discord id of the player.
        /// </summary>
        public ulong DiscordId { get; }

        /// <summary>
        ///     Gets the score saber id of the player.
        /// </summary>
        public long ScoresaberId { get; }

        /// <summary>
        ///     Gets or Sets the score saber rank of the player.
        /// </summary>
        public int Rank { get; set; }
    }
}