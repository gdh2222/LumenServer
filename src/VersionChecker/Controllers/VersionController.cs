using CommonLib;
using CommonLib.Protocols;
using CommonLib.Protocols.VersionChecker;
using DBMediator.Contexts;
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
            Res_VersionCheck resultparam = new Res_VersionCheck();

            using (var adminDbContext = new DbContextAdmin())
            {
                #region CheckVersion
                Version? clientVer;
                Version? serverVer;
                if (false == Version.TryParse(reqParam.Version, out clientVer))
                {
                    return SendError(RESPONSE_CODE.CRITICAL);
                }

                string validVersion = adminDbContext.GetGameVersionByMarket((int)reqParam.MarketType);
                if (!string.IsNullOrEmpty(validVersion))
                {
                    if (false == Version.TryParse(validVersion, out serverVer))
                    {
                        return SendError(RESPONSE_CODE.CRITICAL);
                    }

                    if (serverVer > clientVer)
                    {
                        // 강제업데이트 필요
                        // 강제업데이트 필요시, DB 에서 허용하는 최소 버전을 입력
                        return SendError(RESPONSE_CODE.NEED_UPDATE);
                    }
                }
                #endregion


                #region Maintanence
                var mtinfo = adminDbContext.GetMaintanenceSchedule();
                if (null != mtinfo)
                {
                    resultparam.MaintenanceInfo = new MaintenanceStatusInfo();
                    resultparam.MaintenanceInfo.IsMaintenace = true;
                    resultparam.MaintenanceInfo.StartDate = mtinfo.Startdt;
                    resultparam.MaintenanceInfo.EndDate = mtinfo.Enddt;
                    resultparam.MaintenanceInfo.RemainSec = (int)(mtinfo.Enddt - DateTime.Now).TotalSeconds;
                    return SendSuccess(resultparam);
                }
                #endregion


                #region Redirect
                var redirectInfo = adminDbContext.GetRedirectionInfo((int)reqParam.MarketType, reqParam.Version);
                if (null != redirectInfo)
                {
                    resultparam.IsRedirect = true;
                    resultparam.RedirectUrl = redirectInfo.Authurl;
                    return SendSuccess(resultparam);
                }
                #endregion

                #region  Make CDN Url
                switch (reqParam.MarketType)
                {
                    case MARKET_TYPE.IOS:
                        {
                            string url = string.Format("{0}{1}/iOS/", GlobalConfig.CDN_Prefix, reqParam.Version);
                            resultparam.CDNUrl = url;
                            resultparam.AseetFileListName = GlobalConfig.AseetFileName;
                        }
                        break;

                    default:
                        {
                            string url = string.Format("{0}{1}/Android/", GlobalConfig.CDN_Prefix, reqParam.Version);
                            resultparam.CDNUrl = url;
                            resultparam.AseetFileListName = GlobalConfig.AseetFileName;
                        }
                        break;
                }

                var cdnsubinfo = adminDbContext.GetCDNSubSpec((int)reqParam.MarketType, reqParam.Version);
                if (null != cdnsubinfo)
                {
                    if (string.IsNullOrEmpty(cdnsubinfo.Subfolder) == false)
                    {
                        string f_subs = cdnsubinfo.Subfolder.Trim();
                        if (string.IsNullOrEmpty(f_subs) == false)
                        {
                            switch (reqParam.MarketType)
                            {
                                case MARKET_TYPE.IOS:
                                    {
                                        string url = string.Format("{0}{1}/{2}/iOS/", GlobalConfig.CDN_Prefix, reqParam.Version, f_subs);
                                        resultparam.CDNUrl= url;
                                        resultparam.AseetFileListName = GlobalConfig.AseetFileName;
                                    }
                                    break;

                                default:
                                    {

                                        string url = string.Format("{0}{1}/{2}/Android/", GlobalConfig.CDN_Prefix, reqParam.Version, f_subs);
                                        resultparam.CDNUrl = url;
                                        resultparam.AseetFileListName = GlobalConfig.AseetFileName;
                                    }
                                    break;
                            }
                        }
                    }
                }

                #endregion
            }

            return SendSuccess(resultparam);
        }
    }
}
