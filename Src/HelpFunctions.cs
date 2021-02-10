using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Bot
{
    /// <summary>
    ///     Class that provides help functions.
    /// </summary>
    public static class HelpFunctions
    {
        private static readonly ExeConfigurationFileMap PlayersMap = new() { ExeConfigFilename = @"players.xml" };
        private static readonly ExeConfigurationFileMap PlayerNamesMap = new() { ExeConfigFilename = @"playernames.xml" };
        private static readonly ExeConfigurationFileMap TrackedPlayersMap = new() { ExeConfigFilename = @"trackedplayers.xml" };
        private static readonly ExeConfigurationFileMap PlayerRanksMap = new() { ExeConfigFilename = @"playerranks.xml" };

        /// <summary>
        ///     Loads the players.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the players.xml file. </returns>
        public static Configuration LoadPlayers()
        {
            CheckPlayers();
            Configuration players = ConfigurationManager.OpenMappedExeConfiguration(PlayersMap, ConfigurationUserLevel.None);

            return players;
        }

        /// <summary>
        ///     Loads the playernames.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the playernames.xml file. </returns>
        public static Configuration LoadPlayerNames()
        {
            CheckPlayerNames();
            Configuration playerNames = ConfigurationManager.OpenMappedExeConfiguration(PlayerNamesMap, ConfigurationUserLevel.None);

            return playerNames;
        }

        /// <summary>
        ///     Loads the trackedplayers.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the trackedplayers.xml file. </returns>
        public static Configuration LoadTrackedPlayers()
        {
            CheckTrackedPlayers();
            Configuration trackedPlayers = ConfigurationManager.OpenMappedExeConfiguration(TrackedPlayersMap, ConfigurationUserLevel.None);

            return trackedPlayers;
        }

        /// <summary>
        ///     Loads the playerranks.xml file.
        /// </summary>
        /// <returns> A Configuration object with the data of the playerranks.xml file. </returns>
        public static Configuration LoadPlayerRanks()
        {
            CheckPlayerRanks();
            Configuration playerRanks = ConfigurationManager.OpenMappedExeConfiguration(PlayerRanksMap, ConfigurationUserLevel.None);

            return playerRanks;
        }

        private static void CheckPlayers()
        {
            if (!File.Exists("players.xml"))
            {
                Console.WriteLine("Players file \"players.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                sb.AppendLine(
                    "<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\">" +
                    "</configuration>");

                File.WriteAllText(@"players.xml", sb.ToString());

                Configuration players = ConfigurationManager.OpenMappedExeConfiguration(PlayersMap, ConfigurationUserLevel.None);

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

                sb.AppendLine(
                    "<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\">" +
                    "</configuration>");

                File.WriteAllText(@"playernames.xml", sb.ToString());

                Configuration playerNames = ConfigurationManager.OpenMappedExeConfiguration(PlayerNamesMap, ConfigurationUserLevel.None);

                playerNames.Save();

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

                sb.AppendLine(
                    "<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\">" +
                    "</configuration>");

                File.WriteAllText(@"trackedplayers.xml", sb.ToString());

                Configuration trackedPlayers = ConfigurationManager.OpenMappedExeConfiguration(TrackedPlayersMap, ConfigurationUserLevel.None);

                trackedPlayers.Save();

                Console.WriteLine("trackedplayers.xml successfully created.");
            }
        }

        private static void CheckPlayerRanks()
        {
            if (!File.Exists("playerranks.xml"))
            {
                Console.WriteLine("Playerranks file \"playerranks.xml\" does not exist. Creating...");

                StringBuilder sb = new ();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                sb.AppendLine(
                    "<configuration xmlns=\"http://schemas.microsoft.com/.NetConfiguration/v2.0\">" +
                    "</configuration>");

                File.WriteAllText(@"playerranks.xml", sb.ToString());

                Configuration playerRanks = ConfigurationManager.OpenMappedExeConfiguration(PlayerRanksMap, ConfigurationUserLevel.None);

                playerRanks.Save();

                Console.WriteLine("playerranks.xml successfully created.");
            }
        }
    }
}