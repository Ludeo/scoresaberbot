using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Bot
{
    /// <summary>
    ///     Class that provides help functions.
    /// </summary>
    public class HelpFunctions
    {
        /// <summary>
        ///     Loads the config.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the config.xml file. </returns>
        public static Configuration LoadConfig()
        {
            CheckConfig();
            ExeConfigurationFileMap configMap = new () { ExeConfigFilename = @"config.xml" };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            return config;
        }

        /// <summary>
        ///     Loads the players.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the players.xml file. </returns>
        public static Configuration LoadPlayers()
        {
            CheckPlayers();
            ExeConfigurationFileMap playersMap = new () { ExeConfigFilename = @"players.xml" };
            Configuration players = ConfigurationManager.OpenMappedExeConfiguration(playersMap, ConfigurationUserLevel.None);

            return players;
        }

        /// <summary>
        ///     Loads the playernames.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the playernames.xml file. </returns>
        public static Configuration LoadPlayerNames()
        {
            CheckPlayerNames();
            ExeConfigurationFileMap playerNamesMap = new () { ExeConfigFilename = @"playernames.xml" };
            Configuration playernames = ConfigurationManager.OpenMappedExeConfiguration(playerNamesMap, ConfigurationUserLevel.None);

            return playernames;
        }

        /// <summary>
        ///     Loads the trackedplayers.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the trackedplayers.xml file. </returns>
        public static Configuration LoadTrackedPlayers()
        {
            CheckTrackedPlayers();
            ExeConfigurationFileMap trackedPlayersMap = new () { ExeConfigFilename = @"trackedplayers.xml" };
            Configuration trackedPlayers = ConfigurationManager.OpenMappedExeConfiguration(trackedPlayersMap, ConfigurationUserLevel.None);

            return trackedPlayers;
        }

        private static void CheckConfig()
        {
            if (!File.Exists("config.xml"))
            {
                Console.WriteLine("Config file \"config.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sb.AppendLine("<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\"></configuration>");
                File.WriteAllText(@"config.xml", sb.ToString());

                ExeConfigurationFileMap configMap = new () { ExeConfigFilename = @"config.xml" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                config.AppSettings.Settings.Add("token", string.Empty);
                config.AppSettings.Settings.Add("prefix", "b!");
                config.AppSettings.Settings.Add("adminid", string.Empty);
                config.AppSettings.Settings.Add("trackserver", string.Empty);
                config.AppSettings.Settings.Add("trackchannel", string.Empty);

                config.Save();

                Console.WriteLine("Config successfully created. Make sure to edit the values before starting the " +
                                  "bot again. Exiting...");
                Environment.Exit(0);
            }
        }

        private static void CheckPlayers()
        {
            if (!File.Exists("players.xml"))
            {
                Console.WriteLine("Players file \"players.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sb.AppendLine("<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\"></configuration>");
                File.WriteAllText(@"players.xml", sb.ToString());

                ExeConfigurationFileMap playersMap = new () { ExeConfigFilename = @"players.xml" };
                Configuration players = ConfigurationManager.OpenMappedExeConfiguration(playersMap, ConfigurationUserLevel.None);

                players.AppSettings.Settings.Add("311861142114926593", "76561198143629166");

                players.Save();

                Console.WriteLine("players.xml successfully created.");
            }
        }

        private static void CheckPlayerNames()
        {
            if (!File.Exists("playernames.xml"))
            {
                Console.WriteLine("Playernames file \"playernames.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sb.AppendLine("<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\"></configuration>");
                File.WriteAllText(@"playernames.xml", sb.ToString());

                ExeConfigurationFileMap playerNamesMap = new () { ExeConfigFilename = @"playernames.xml" };
                Configuration playernames = ConfigurationManager.OpenMappedExeConfiguration(playerNamesMap, ConfigurationUserLevel.None);

                playernames.AppSettings.Settings.Add("76561198143629166", "Ludeo");

                playernames.Save();

                Console.WriteLine("playernames.xml successfully created.");
            }
        }

        private static void CheckTrackedPlayers()
        {
            if (!File.Exists("trackedplayers.xml"))
            {
                Console.WriteLine("Trackedplayers file \"trackedplayers.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                sb.AppendLine("<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\"></configuration>");
                File.WriteAllText(@"trackedplayers.xml", sb.ToString());

                ExeConfigurationFileMap trackedPlayersMap = new () { ExeConfigFilename = @"trackedplayers.xml" };
                Configuration trackedPlayers = ConfigurationManager.OpenMappedExeConfiguration(trackedPlayersMap, ConfigurationUserLevel.None);

                trackedPlayers.AppSettings.Settings.Add("76561198143629166", "2021-01-25T01:36:25.000Z");

                trackedPlayers.Save();

                Console.WriteLine("trackedplayers.xml successfully created.");
            }
        }
    }
}