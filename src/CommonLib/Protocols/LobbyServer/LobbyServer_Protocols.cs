using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Protocols.LobbyServer
{

    public class Req_Login
    {
        public PLATFORM_TYPE PlatformType { get; set; }
        public string PlatformId { get; set; } = string.Empty;
    }

    public class Res_Login
    {
        public long AccountIdx { get; set; }

    }


}
