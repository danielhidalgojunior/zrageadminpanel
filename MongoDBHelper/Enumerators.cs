using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper
{
    public enum Privilege
    {
        Helper,         // 0
        Moderator,      // 1
        Administrator,  // 2
        Master          // 3
    }
    public enum NoticeType
    {
        PlayerConnected,
        NextMapSelected,
        MapChanged,
        OnlinePlayersValueReached,
        ServerNotResponding,
        LevelOfMapReached
    }

    public enum EventType
    {
        ToastWarning,
        CommandToServer
    }
}
