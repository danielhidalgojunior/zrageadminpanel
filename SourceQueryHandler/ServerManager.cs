using MongoDBHelper.Models;
using QueryMaster;
using QueryMaster.GameServer;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SourceQueryHandler
{
    public static class ServerManager
    {
        public static bool ThrowExceptions = false;
        static Server server { get; set; }
        public static string Ip { get; set; }
        public static ushort Port { get; set; }
        public static string RconPassword { get; set; }

        public static ServerInfo Info
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetInfo();
                }
            }
        }
        public static QueryMasterCollection<PlayerInfo> Players
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetPlayers();
                }
            }
        }

        public static QueryMasterCollection<Rule> Rules
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetRules();
                }
            }
        }

        public static void Initialize(RconModel auth)
        {
            Ip = auth.Ip;
            Port = auth.Port;
            RconPassword = auth.RconPassword;

            server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, new IPEndPoint(IPAddress.Parse(Ip), Port), false, 3000, 4000, 1, ThrowExceptions);
            server.GetControl(RconPassword);

            Task.Run(() => Initialize());
        }

        static async Task Initialize()
        {
            await SendCommand("status");
        }

        public static async Task<string> SendCommand(string command)
        {
            string response = null;

            try
            {
                if ((Variables.LoggedUser as UserModel).IsCommandAvaliable(command))
                {
                    response = await server.Rcon.SendCommandAsync(command);
                }
                else
                {
                    MessageBox.Show($"You don't have permission to use {command.Split(' ')[0]}", "Permission denied", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}: Command sent and failed with exception {ex.ToString()}");
            }

            return response;
        }

        public static void DisposeServer() => server?.Dispose();
    }
}
