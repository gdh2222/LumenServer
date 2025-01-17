using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public enum PLATFORM_TYPE
    {

        GUEST       = 1,
        GOOLEPLAY   = 10,
        APPLE       = 20,
    }

    public enum MARKET_TYPE
    {
        GOOGLE_PLAY = 1,
        IOS       = 2,
    }

    public enum OS_TYPE
    {
        ANDROID     = 1,
        APPLE       = 2,
    }

    public enum SANCTION_TYPE
    {
        /// <summary>
        /// 제재가 없는 상태입니다.
        /// </summary>
        NO_SANCTION,

        /// <summary>
        /// 일정 시간 동안 제재가 적용되는 상태입니다.
        /// </summary>
        TEMPORARY_SANCTION
    }


}
