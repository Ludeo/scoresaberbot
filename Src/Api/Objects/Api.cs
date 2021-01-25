using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Class for the Score Saber Api.
    /// </summary>
    public class Api
    {
        private const string BaseUrl = "https://new.scoresaber.com/api/";

        private readonly HttpClient httpClient = new ();

        private int rateLimitReset;

        /// <summary>
        ///     Gets the remaining api calls for the score saber api.
        ///     <remarks> The limit is set to 80 because that's the limit that score saber sets for a user.
        ///         Don't change the limit to anything higher than 80.
        ///         If you do it, it can cause problems and in the worst case get you banned from using the api!
        ///     </remarks>
        /// </summary>
        public int RateLimitRemaining { get; private set; } = 80;

        /// <summary>
        ///     Gets a player by his Score Saber Id.
        /// </summary>
        /// <param name="playerId"> The Score Saber Id of the player. </param>
        /// <returns> Returns a Player object of the player. </returns>
        public async Task<Player> GetPlayerAsync(long playerId)
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}player/{playerId}/full")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            Player player = JsonSerializer.Deserialize<Player>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return player;
        }

        /// <summary>
        ///     Gets the TopScores of a player from a specific pageNumber.
        /// </summary>
        /// <param name="playerId"> The id of the player. </param>
        /// <param name="pageNumber"> The page number from where it should get the TopScores. </param>
        /// <returns> Returns a TopScores object from the player. </returns>
        public async Task<TopScores> GetTopScoresOfPlayerAsync(long playerId, int pageNumber = 1)
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}player/{playerId}/scores/top/{pageNumber}")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            TopScores topScores = JsonSerializer.Deserialize<TopScores>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return topScores;
        }

        /// <summary>
        ///     Gets the RecentScores of a player from a specific pageNumber.
        /// </summary>
        /// <param name="playerId"> The id of the player. </param>
        /// <param name="pageNumber"> The page number from where it should get the RecentScores. </param>
        /// <returns> Returns a RecentScores object from the player. </returns>
        public async Task<RecentScores> GetRecentScoresOfPlayerAsync(long playerId, int pageNumber = 1)
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}player/{playerId}/scores/recent/{pageNumber}")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            RecentScores recentScores = JsonSerializer.Deserialize<RecentScores>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return recentScores;
        }

        /// <summary>
        ///     Gets an array of players after searching for the name.
        /// </summary>
        /// <param name="searchInput"> The name that should be searched for. </param>
        /// <returns> Returns a PlayerSearch object. </returns>
        public async Task<PlayerSearch> SearchForPlayerAsync(string searchInput)
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}players/by-name/{searchInput}")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            PlayerSearch players = JsonSerializer.Deserialize<PlayerSearch>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return players;
        }

        /// <summary>
        ///     Gets an array of players from a specific page of the leaderboard.
        /// </summary>
        /// <param name="leaderboardPage"> The number of the page from the leaderboard. </param>
        /// <returns> Returns a LeaderboardPages object. </returns>
        public async Task<PlayerRanking> GetPlayersFromLeaderBoardAsync(int leaderboardPage = 1)
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}players/{leaderboardPage}")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            PlayerRanking players = JsonSerializer.Deserialize<PlayerRanking>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return players;
        }

        /// <summary>
        ///     Gets the number of pages that exist in the leaderboard.
        /// </summary>
        /// <returns> Returns a LeaderboardPages object. </returns>
        public async Task<LeaderboardPages> GetLeaderboardPagesAsync()
        {
            if (!this.Allowed())
            {
                return null;
            }

            HttpResponseMessage response = this.httpClient
                                               .GetAsync(
                                                   $"{BaseUrl}players/pages")
                                               .Result;

            string responseString = await response.Content.ReadAsStringAsync();

            LeaderboardPages pages = JsonSerializer.Deserialize<LeaderboardPages>(responseString);
            ResponseHeader header = new (response.Headers.ToString());

            this.RateLimitRemaining = header.RateLimitRemaining;
            this.rateLimitReset = header.RateLimitReset;

            return pages;
        }

        private bool Allowed()
        {
            if (this.RateLimitRemaining <= 0)
            {
                if (this.rateLimitReset >= DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return false;
                }

                return true;
            }

            return true;
        }
    }
}