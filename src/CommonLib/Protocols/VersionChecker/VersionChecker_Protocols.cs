namespace CommonLib.Protocols.VersionChecker
{
    public class Req_VersionCheck : REQ_Header
    {
        public MARKET_TYPE MarketType { get; set; } = MARKET_TYPE.GOOGLE_PLAY;
        public string Version { get; set; } = string.Empty;
    }

    public class Res_VersionCheck : RES_Header
    {
        /// <summary>
        /// 마켓으로 보낼지 여부
        /// 강제업데이트( 메이저버전 업데이트 )
        /// </summary>
        public bool IsGoMarket { get; set; }

        /// <summary>
        /// 마켓검수용
        /// 정해진 게임서버로 리다이렉션 시키는 기능
        /// </summary>
        public bool IsRedirect { get; set; }

        /// <summary>
        /// 리다이렉션 주소
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;

        /// <summary>
        /// 점검 상태정보
        /// </summary>
        public MaintenanceStatusInfo? MaintenanceInfo { get; set; }

        /// <summary>
        /// CDN 주소
        /// </summary>
        public string CDNUrl { get; set; } = string.Empty;

        /// <summary>
        /// Unity Asset Filelist 이름 또는 파일별 버전관리하는 파일 이름
        /// </summary>
        public string AseetFileListName { get; set; } = string.Empty;
    }


}
