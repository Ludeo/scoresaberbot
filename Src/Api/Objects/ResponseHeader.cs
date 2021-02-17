using System;

namespace Bot.Api.Objects
{
    /// <summary>
    ///     Object for the ResponseHeader of an Api Call.
    /// </summary>
    public class ResponseHeader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResponseHeader"/> class.
        /// </summary>
        /// <param name="header"> The header of the response from the api call. </param>
        public ResponseHeader(string header)
        {
            foreach (string line in header.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.StartsWith("X-RateLimit-Limit"))
                {
                    int a = line.IndexOf(":", StringComparison.Ordinal);
                    this.RateLimit = int.Parse(line.Substring(a + 1));
                }
                else if (line.StartsWith("X-RateLimit-Remaining"))
                {
                    int a = line.IndexOf(":", StringComparison.Ordinal);
                    this.RateLimitRemaining = int.Parse(line.Substring(a + 1));
                }
                else if (line.StartsWith("X-RateLimit-Reset"))
                {
                    int a = line.IndexOf(":", StringComparison.Ordinal);
                    this.RateLimitReset = int.Parse(line.Substring(a + 1));
                }
            }
        }

        /// <summary>
        ///     Gets the remaining amount of calls you can make till the reset.
        /// </summary>
        public int RateLimitRemaining { get; }

        /// <summary>
        ///     Gets the time of the limit reset.
        /// </summary>
        public int RateLimitReset { get; }

        /// <summary>
        ///     Gets the maximum RateLimit of calls you can make to the Api.
        /// </summary>
        private int RateLimit { get; }
    }
}