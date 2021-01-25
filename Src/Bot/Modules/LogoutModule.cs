using System;
using System.Configuration;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Bot.Modules
{
    /// <summary>
    ///     Module for the logout command.
    /// </summary>
    public class LogoutModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     Function that gets executed when the admin uses the logout command.
        /// </summary>
        /// <returns> . </returns>
        [Command("logout")]
        public async Task LogoutAsync()
        {
            Configuration config = HelpFunctions.LoadConfig();

            if (this.Context.Message.Author.Id.ToString() != config.AppSettings.Settings["adminid"].Value)
            {
                return;
            }

            await Program.Client.LogoutAsync();
            Environment.Exit(0);
        }
    }
}