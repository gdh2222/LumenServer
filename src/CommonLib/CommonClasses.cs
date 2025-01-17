using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class MaintenanceStatusInfo
    {
        public bool IsMaintenace;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int RemainSec { get; set; }
    }

    /// <summary>
    /// 계정 제재 정보를 나타내는 클래스입니다.
    /// </summary>
    public class AccountSanction
    {
        /// <summary>
        /// 계정 번호
        /// </summary>
        public long AccIdx { get; set; }

        /// <summary>
        /// 계정에 적용된 제재 타입
        /// </summary>
        public SANCTION_TYPE SanctionType { get; set; }

        /// <summary>
        /// 제재 메시지 키
        /// </summary>
        public int SanctionMessageKey { get; set; }

        /// <summary>
        /// 제재 종료 날짜
        /// </summary>
        public DateTime? SanctionEndDate { get; set; }
    }

}
