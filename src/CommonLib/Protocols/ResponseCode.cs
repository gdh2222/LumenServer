using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Protocols
{
    public enum RESPONSE_CODE : int
    {
        SUCCESS             = 0,

        #region 타이틀로 보내야되는 에러
        /// <summary>
        /// 심각한 에러
        /// </summary>
        CRITICAL = 1,

        /// <summary>
        /// 서버내부에서 예외발생
        /// </summary>
        EXCEPTION = 2,

        /// <summary>
        /// 로그인세션 만료, 보통 중복로그인시 발생
        /// </summary>
        EXPIRE_SESSION = 3,

        /// <summary>
        /// 프로토콜에러, 최신버전 안받은경우
        /// </summary>
        NA_PROTOCOL = 4,

        
        #endregion

        /// <summary>
        /// 업데이트 필요
        /// </summary>
        NEED_UPDATE = 10,


    }
}
