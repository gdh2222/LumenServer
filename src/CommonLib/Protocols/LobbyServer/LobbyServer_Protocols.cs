namespace CommonLib.Protocols.LobbyServer
{
    public class Req_Login : REQ_Header
    {
        public PLATFORM_TYPE PlatformType { get; set; }
        public string PlatformId { get; set; } = string.Empty;
    }

    public class Res_Login : RES_Header
    {
        public long AccountIdx { get; set; }

    }


}
