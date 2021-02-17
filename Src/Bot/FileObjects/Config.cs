using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Bot.FileObjects
{
    /// <summary>
    ///     Class that represents the config file for the Bot.
    /// </summary>
    public class Config
    {
        /// <summary>
        ///     Gets the Default Config object.
        /// </summary>
        public static Config Default { get; } = GetConfig();

        /// <summary>
        ///     Gets or sets the discord bot token.
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        ///     Gets or sets the discord bot prefix.
        /// </summary>
        [JsonPropertyName("prefix")]
        public string Prefix { get; set; } = "b!";

        /// <summary>
        ///     Gets or sets the discord id of the bot admin.
        /// </summary>
        [JsonPropertyName("adminid")]
        public ulong AdminId { get; set; }

        /// <summary>
        ///     Gets or sets the discord id of the server where the tracked scores get send in.
        /// </summary>
        [JsonPropertyName("trackserver")]
        public ulong TrackServer { get; set; }

        /// <summary>
        ///     Gets or sets the discord id of the channel where the tracked scores get send in.
        /// </summary>
        [JsonPropertyName("trackchannel")]
        public ulong TrackChannel { get; set; }

        private static Config GetConfig()
        {
            if (!File.Exists("config.json"))
            {
                Config configFile = new ();
                string jsonData = JsonSerializer.Serialize(configFile);
                File.WriteAllText("config.json", jsonData);

                return configFile;
            }

            return JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
        }
    }
}