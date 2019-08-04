using QueryMaster;
using QueryMaster.GameServer;
using SourceQueryHandler;
using SourceQueryHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceQueryHandler.models
{
    public class Player
    {
        public int UserId { get; set; }
        public TeamType Team { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public long Score { get; set; }
        public int Ping { get; set; }
        public string Ip { get; set; }
        public string State { get; set; }
        public TimeSpan OnlineTime { get; set; }
        public bool Alive { get; set; }
        public int OfflineMarkerCounter { get; set; }

        public static List<Player> GetPlayersInfo(QueryMasterCollection<PlayerInfo> players, string status)
        {
            var result = new List<Player>();

            foreach (var p in players)
            {
                if (p.Name.Length == 0)
                    continue;

                var player = new Player();
                var statusdata = ExtractData(GetLineOfPlayer(p.Name, status), player);

                player.Name = p.Name;
                player.OnlineTime = p.Time;
                player.Score = p.Score;
                player.Ip = statusdata.Ip;
                player.Ping = statusdata.Ping;
                player.UserId = statusdata.UserId;
                player.State = statusdata.State;
                player.SteamId = statusdata.SteamId;
                

                result.Add(player);
            }

            return result;
        }

        public static string GetLineOfPlayer(string playername, string status)
        {
            var list = status
                .Split('\n')
                .Where(x => x.StartsWith("#"))
                .Where(x => x.Contains($"\"{playername}\""))
                .ToList();
            return list.SingleOrDefault();
        }

        public static Player ExtractData(string statusline, Player player)
        {
            statusline = RemoveKnownInfo(statusline, player);

            if (statusline != null)
            {
                var statusarray = statusline.Split(' ');

                player.UserId = Convert.ToInt32(statusarray[0]);
                player.State = statusarray[6];
                player.SteamId = statusarray[2];
                player.Ping = Convert.ToInt32(statusarray[4]);
                player.Ip = statusarray[8];
            }


            return player;
        }

        private static string RemoveKnownInfo(string statusline, Player player)
        {
            if (statusline == null)
                return null;

            statusline = statusline
                .Replace("# ", "")
                .Replace($"\"{player.Name}\" ", "");

            return statusline;
        }
    }
}
