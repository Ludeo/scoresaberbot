using System.Threading.Tasks;
using Bot.Api.Objects;
using Discord;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the top5 command.
    /// </summary>
    public class Top5Module : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when someone uses the top5 command.
        /// </summary>
        /// <returns> . </returns>
        [Command("top5")]
        public async Task Top5Async()
        {
            Api.Objects.Api api = Program.GetApi();

            PlayerRanking players = await api.GetPlayersFromLeaderBoardAsync();

            SearchedPlayer[] playerArray = players.Players;

            for (int i = 0; i < 5; i++)
            {
                SearchedPlayer player = playerArray[i];
                EmbedBuilder embedBuilder = new ()
                {
                    Title = player.PlayerName,
                    ThumbnailUrl = "https://new.scoresaber.com" + player.Avatar,
                    Url = "https://new.scoresaber.com/u/" + player.PlayerId,
                    Color = Color.Green,
                };

                embedBuilder.AddField("Rank", player.Rank, true);
                embedBuilder.AddField("PP", player.Pp, true);
                embedBuilder.AddField("Country", player.Country, true);

                string weeklyChangeText = "```diff\n- " + -player.RankDifference + "```";

                if (player.RankDifference >= 0)
                {
                    weeklyChangeText = "```diff\n+ " + player.RankDifference + "```";
                }

                embedBuilder.AddField("Weekly Change", weeklyChangeText, true);

                await this.Context.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
            }
        }
    }
}