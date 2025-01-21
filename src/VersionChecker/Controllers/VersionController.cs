using CommonLib;
using CommonLib.Protocols;
using CommonLib.Protocols.VersionChecker;
using DBMediator.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerCommon;

namespace VersionChecker.Controllers
{
    [ApiController]
    [Route("api/version")]
    public class VersionController : LunarController
    {
        private readonly ILogger<VersionController> _logger;
        private readonly DbContextFactory _factory;

        public VersionController(ILogger<VersionController> logger, DbContextFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost("check")]
        public async Task<IActionResult> Check([FromBody] Req_VersionCheck reqParam)
        {
            Res_VersionCheck resultparam = new Res_VersionCheck();

            using (var adminDbContext = _factory.Create<DbContextAdmin>())
            {
                #region CheckVersion
                Version? clientVer;
                Version? serverVer;
                if (false == Version.TryParse(reqParam.Version, out clientVer))
                {
                    return SendError(RESPONSE_CODE.CRITICAL);
                }

                var dbVersion = await adminDbContext.Appvalidversions.FirstOrDefaultAsync(r => r.Mkt == (int)reqParam.MktType);
                if (null != dbVersion)
                {
                    if (false == Version.TryParse(dbVersion.Validver, out serverVer))
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
                var mtinfo = await adminDbContext.Maintanenceschedules.FirstOrDefaultAsync(r => r.Startdt <= DateTime.Now && r.Enddt >= DateTime.Now);
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
                var redirectInfo = await adminDbContext.Redirectionsinfos.FirstOrDefaultAsync(r => r.Mkt == (int)reqParam.MktType && r.Version == reqParam.Version);
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

                var cdnsubinfo = await adminDbContext.Cdnsubspecs.FirstOrDefaultAsync(r => r.Mkt == (int)reqParam.MarketType && r.Version == reqParam.Version);
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
                                        resultparam.CDNUrl = url;
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
