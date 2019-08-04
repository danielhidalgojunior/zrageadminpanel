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
    public class ServerManager
    {
        public static bool ThrowExceptions = false;
        Server server { get; set; }
        public string Ip { get; set; }
        public ushort Port { get; set; }
        public string RconPassword { get; set; }

        public ServerInfo Info
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetInfo();
                }
            }
        }
        public QueryMasterCollection<PlayerInfo> Players
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetPlayers();
                }
            }
        }

        public QueryMasterCollection<Rule> Rules
        {
            get
            {
                using (var server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, Ip, Port, false, 1000, 1000, 3, ThrowExceptions))
                {
                    return server.GetRules();
                }
            }
        }

        public ServerManager(RconModel auth)
        {
            Ip = auth.Ip;
            Port = auth.Port;
            RconPassword = auth.RconPassword;

            server = ServerQuery.GetServerInstance(QueryMaster.EngineType.Source, new IPEndPoint(IPAddress.Parse(Ip), Port), false, 3000, 4000, 1, ThrowExceptions);
            server.GetControl(RconPassword);

            Task.Run(() => Initialize());
        }

        public async Task Initialize()
        {
            await SendCommand("status");
        }

        public async Task<string> SendCommand(string command)
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

        public void DisposeServer() => server?.Dispose();
    }
}
