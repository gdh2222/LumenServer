using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMediator.DBModels
{
    public class GameVersion
    {
        public int OsType { get; set; }
        public string Version { get; set; } = string.Empty;
    }

    public class GameMaintenance
    {
        public int ReasonCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class RedirectInfo
    {
        public int OsType { get; set; }
        public string Version { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
