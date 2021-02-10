using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot
{
    public class Config
    {
        public static Config Default { get; } = GetConfig();

        private static Config GetConfig()
        {
            if (!File.Exists("config.json"))
            {
                Config configFile = new ();
                string jsonData = JsonSerializer.Serialize(configFile);
                File.WriteAllText("config.json", jsonData);

                return configFile;
            }

            return JsonSerializer.Deserialize<Config>("config.json");
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("prefix")]
        public string Prefix { get; set; } = "b!";

        [JsonPropertyName("adminid")]
        public ulong AdminId { get; set; }

        [JsonPropertyName("trackserver")]
        public ulong TrackServer { get; set; }

        [JsonPropertyName("trackchannel")]
        public ulong TrackChannel { get; set; }
    }
}