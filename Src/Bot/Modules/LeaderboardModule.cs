using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bot.Api.Objects;
using Bot.Bot.FileObjects;
using Bot.Bot.Objects;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the leaderboard command.
    /// </summary>
    public class LeaderboardModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the leaderboard command.
        /// </summary>
        /// <returns> An empty task. </returns>
        [Command("leaderboard")]
        public async Task LeaderboardAsync()
        {
            List<PlayerInformation> playerInformations = PlayerInformation.FromJson();

            List<LinkedPlayer> linkedPlayers = LinkedPlayer.FromJson();

            List<LeaderboardPlayer> leaderboardPlayers = new ();

            Api.Objects.Api api = Program.GetApi();

            foreach (LinkedPlayer linkedPlayer in linkedPlayers!
                .Where(linkedPlayer => this.Context.Guild.GetUser(linkedPlayer.DiscordId) != null))
            {
                if (playerInformations!.All(player => player.Id != linkedPlayer.ScoreSaberId))
                {
                    Player player = await api.GetPlayerAsync(linkedPlayer.ScoreSaberId);
                    PlayerInfo playerInfo = player.PlayerInfo;
                    playerInformations.Add(new PlayerInformation
                    {
                        Id = linkedPlayer.ScoreSaberId,
                        Name = playerInfo.PlayerName,
                        Rank = playerInfo.Rank,
                    });
                }

                PlayerInformation playerInformation =
                    playerInformations.Find(player => player.Id == linkedPlayer.ScoreSaberId);

                LeaderboardPlayer leaderboardPlayer = new ()
                {
                    DiscordId = linkedPlayer.DiscordId,
                    ScoresaberId = linkedPlayer.ScoreSaberId,
                    Name = playerInformation.Name,
                    Rank = playerInformation.Rank,
                };

                leaderboardPlayers.Add(leaderboardPlayer);
            }

            await File.WriteAllTextAsync(
                "playerinformation.json", JsonSerializer.Serialize(playerInformations));

            leaderboardPlayers = leaderboardPlayers.OrderBy(x => x.Rank).ToList();

            StringBuilder sb = new ();
            sb.AppendLine("```autohotkey");
            sb.AppendLine("+-------------------------------------------------+---------+");
            sb.AppendLine($"| {"User",-48}| {"Rank",-7} |");
            sb.AppendLine("+=================================================+=========+");

            for (int i = 0; i < leaderboardPlayers.Count; i++)
            {
                if (i == 10)
                {
                    break;
                }

                LeaderboardPlayer player = leaderboardPlayers[i];

                SocketGuildUser user = this.Context.Guild.GetUser(player.DiscordId);
                string discordName = $"{user.Username}#{user.DiscriminatorValue}";

                string scoreSaberName = $"({player.Name})";

                string fullName = $"{discordName} {scoreSaberName}";

                if (fullName.Length > 46)
                {
                    sb.AppendLine(
                        i == 9
                            ? $"| {i + 1}. {discordName,-44}| {player.Rank,-8}|"
                            : $"| {i + 1}. {discordName,-45}| {player.Rank,-8}|");

                    sb.AppendLine($"|  {scoreSaberName,-47}| {string.Empty,-8}|");
                }
                else
                {
                    sb.AppendLine(
                        i == 9
                            ? $"| {i + 1}. {fullName,-44}| {player.Rank,-8}|"
                            : $"| {i + 1}. {fullName,-45}| {player.Rank,-8}|");
                }

                sb.AppendLine("+-------------------------------------------------+---------+");
            }

            int pos =
                leaderboardPlayers.FindIndex(x => x.DiscordId == this.Context.Message.Author.Id);
            string yourPosition = $"Your Position: {pos + 1}/{leaderboardPlayers.Count}";
            sb.AppendLine($"| {yourPosition,-47} | {leaderboardPlayers[pos].Rank,-8}|");
            sb.AppendLine("+-------------------------------------------------+---------+");
            sb.AppendLine("```");

            EmbedBuilder embedBuilder = new ()
            {
                Title = "Score Saber Leaderboard of " + this.Context.Guild.Name,
                Description = sb.ToString(),
                Color = Color.Teal,
            };

            embedBuilder.WithFooter("API Calls Left: " + api.RateLimitRemaining);

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}