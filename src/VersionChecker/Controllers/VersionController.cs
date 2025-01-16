using CommonLib.Protocols;
using CommonLib.Protocols.VersionChecker;
using Microsoft.AspNetCore.Mvc;
using ServerCommon;

namespace VersionChecker.Controllers
{
    [ApiController]
    [Route("api/version")]
    public class VersionController : LunarController
    {
        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("check")]
        public IActionResult Check([FromBody] Req_VersionCheck reqParam)
        {
            Res_VersionCheck response = new Res_VersionCheck();

            using (var adminDbContext = new DBMediator.Contexts.DbContextAdmin())
            {
                #region CheckVersion
                Version? clientVer;
                Version? serverVer;
                if (false == Version.TryParse(reqParam.Version, out clientVer))
                {
                    return SendError(RESPONSE_CODE.INVAILD_VERSION);
                }

                string validVersion = adminDbContext.GetGameVersionByMarket((int)reqParam.MarketType);
                if (!string.IsNullOrEmpty(validVersion))
                {
                    if (false == Version.TryParse(validVersion, out serverVer))
                    {
                        return SendError(RESPONSE_CODE.INVAILD_VERSION);
                    }

                    if (serverVer > clientVer)
                    {
                        // 강제업데이트 필요
                        // 강제업데이트 필요시, DB 에서 허용하는 최소 버전을 입력
                        return SendError(RESPONSE_CODE.INVAILD_VERSION);
                    }
                }
                #endregion


                var redirectInfo = adminDbContext.GetRedirectionInfo((int)reqParam.MarketType, reqParam.Version);
                if (null != redirectInfo)
                {
                    response.IsRedirect = true;
                    response.RedirectUrl = redirectInfo.Authurl;
                    return SendSuccess(response);
                }
            }


            //// 2. Check Maintenance
            //var maintenanceInfo = FakeDB_Service.GetGameVersion((int)ReqParam.MarketType);
            //if(null != maintenanceInfo)
            //{
            //    // TODO : Send error reason code
            //    // 점검중, 시간 및 점검사유코드 전송
            //    return NoContent();
            //}


            //// 3. Redirect Gameserver url
            //var redirectInfo = FakeDB_Service.GetRedirectInfo((int)ReqParam.MarketType, ReqParam.Version);
            //if(null != redirectInfo)
            //{
            //    // TODO : Send error reason code
            //    // 리다이렉트, 원래 게임서버가아닌, 다른서버로 리다이렉트
            //    return NoContent();
            //}

            //// 4. CDN Url 조합
            //// Sample Url
            //// http://localhost:8080/ANDROID/1.0.0
            //string cdnUrl = $"{GlobalConfig.CDNUrl}/{ReqParam.MarketType}/{clientVer.Major}.{clientVer.Minor}.{clientVer.Build}";


            return SendSuccess(response);
        }
    }
}
