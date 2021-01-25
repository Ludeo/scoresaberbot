﻿using System.Configuration;
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
        /// <returns> . </returns>
        [Command("stoptrack")]
        public async Task StopTrackAsync()
        {
            Configuration config = HelpFunctions.LoadConfig();

            if (this.Context.Message.Author.Id.ToString() != config.AppSettings.Settings["adminid"].Value)
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