using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Protocols.VersionChecker
{
    public class Req_VersionCheck
    {
        public OS_TYPE OsType { get; set; } = OS_TYPE.ANDROID;
        public string Version { get; set; } = string.Empty;
    }

    public class Res_VersionCheck
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
        /// 점검 여부
        /// </summary>
        public bool IsMaintenance { get; set; }

        /// <summary>
        /// CDN 주소
        /// </summary>
        public string CDNUrl { get; set; } = string.Empty;
    }


}
