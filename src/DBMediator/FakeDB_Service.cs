using DBMediator.DBModels;

namespace DBMediator
{
    public class FakeDB_Service
    {
        protected static List<GameVersion> GameVersionList = new List<GameVersion>()
        {
            new GameVersion() { OsType = 1, Version = "1.0.0.0" },
            new GameVersion() { OsType = 2, Version = "1.0.0.0" },
        };


        /// <summary>
        /// 점검시간리스트
        /// 테스트시, 시작시간, 종료시간 하드코딩된 부분 수정
        /// </summary>
        protected static List<GameMaintenance> MaintenanceList = new List<GameMaintenance>()
        {
            // new GameMaintenance(){ ReasonCode = 1, StartTime = DateTime.Now, EndTime = DateTime.Parse("2026-01-06 00:00:00") },
            new GameMaintenance(){ ReasonCode = 1, StartTime = DateTime.Parse("1990-01-01"), EndTime = DateTime.Parse("1990-01-01") },
            new GameMaintenance(){ ReasonCode = 1, StartTime = DateTime.Parse("1990-01-01"), EndTime = DateTime.Parse("1990-01-01") },
            new GameMaintenance(){ ReasonCode = 1, StartTime = DateTime.Parse("1990-01-01"), EndTime = DateTime.Parse("1990-01-01") }
        };


        /// <summary>
        /// 리다이렉트
        /// IOS 검수 및 프로덕션 서버에서 정해진 다른 게임서버로 보내기위한 설정
        /// </summary>
        protected static List<RedirectInfo> RedirectList = new List<RedirectInfo>()
        {
            // new RedirectInfo(){ OsType = 1, Version = "1.0.0", RedirectUrl = "http://localhost:8888" },
            // new RedirectInfo(){ OsType = 2, Version = "1.0.0", RedirectUrl = "http://localhost:8888" },
        };

        public static string GetGameVersion(int osType)
        {
            GameVersion? gamever = GameVersionList.FirstOrDefault(r => r.OsType == osType);
            if (null == gamever)
            {
                return string.Empty;
            }

            return gamever.Version;
        }

        public static GameMaintenance? GetMaintenance()
        {
            return MaintenanceList.FirstOrDefault(row => row.StartTime >= DateTime.Now && row.EndTime <= DateTime.Now);
        }

        public static RedirectInfo? GetRedirectInfo(int osType, string clientVersion)
        {
            return RedirectList.FirstOrDefault(row => row.OsType == osType && row.Version == clientVersion);
        }
    }
}
