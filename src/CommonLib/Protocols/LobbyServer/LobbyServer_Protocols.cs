namespace CommonLib.Protocols.LobbyServer
{
    public class Req_Login : REQ_Header
    {
        public int PlatformType { get; set; }

        /// <summary>
        /// 플랫폼 타입에따른 JSON String
        /// </summary>
        public string PlatformId { get; set; } = string.Empty;
    }

    public class Res_Login : RES_Header
    {
        public long AccountIdx { get; set; }

        public string SessionKey { get; set; } = string.Empty;

        /// <summary>
        /// 제재정보, null 아닐시 유저 접속 제한
        /// </summary>
        public AccountSanction? SanctionInfo { get; set; }

    }


}
