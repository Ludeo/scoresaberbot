using System.Configuration;
using System.Threading.Tasks;
using System.Timers;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the stoptrack command.
    /// </summary>
    public class StopTrackModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when the admin uses the stoptrack command.
        /// </summary>
        /// <returns> An empty task. </returns>
        [Command("stoptrack")]
        public async Task StopTrackAsync()
        {
            Config config = Config.Default;

            if (this.Context.Message.Author.Id != config.AdminId)
            {
                return;
            }

            Timer timer = Program.Timer;

            if (timer.Enabled)
            {
                timer.Enabled = false;
            }

            await this.Context.Channel.SendMessageAsync("Tracking stopped");
        }
    }
}