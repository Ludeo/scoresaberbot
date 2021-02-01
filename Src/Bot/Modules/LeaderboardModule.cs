using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Api.Objects;
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
        /// <returns> . </returns>
        [Command("leaderboard")]
        public async Task LeaderboardAsync()
        {
            Configuration playerRanks = HelpFunctions.LoadPlayerRanks();

            Configuration players = HelpFunctions.LoadPlayers();

            List<LeaderboardPlayer> leaderboardPlayers = new ();

            foreach (string key in players.AppSettings.Settings.AllKeys)
            {
                if (this.Context.Guild.GetUser(ulong.Parse(key!)) != null)
                {
                    LeaderboardPlayer player = new (
                        ulong.Parse(key!),
                        long.Parse(players.AppSettings.Settings[key].Value!));

                    leaderboardPlayers.Add(player);
                }
            }

            Api.Objects.Api api = Program.GetApi();

            Configuration playerNames = HelpFunctions.LoadPlayerNames();

            foreach (LeaderboardPlayer player in leaderboardPlayers)
            {
                if (playerRanks.AppSettings.Settings.AllKeys!.Any(key => key == player.ScoresaberId.ToString()))
                {
                    player.Rank = int.Parse(playerRanks.AppSettings.Settings[player.ScoresaberId.ToString()].Value!);
                }
                else
                {
                    Player newPlayer = await api.GetPlayerAsync(player.ScoresaberId);
                    player.Rank = newPlayer.PlayerInfo.Rank;

                    playerRanks.AppSettings.Settings.Add(player.ScoresaberId.ToString(), player.Rank.ToString());

                    if (playerNames.AppSettings.Settings.AllKeys!.All(x => x != player.ScoresaberId.ToString()))
                    {
                        playerNames.AppSettings.Settings
                                   .Add(player.ScoresaberId.ToString(), newPlayer.PlayerInfo.PlayerName);
                    }
                }
            }

            playerRanks.Save();
            playerNames.Save();

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

                string scoreSaberName = $"({playerNames.AppSettings.Settings[player.ScoresaberId.ToString()].Value})";

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
            };

            await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }
    }
}