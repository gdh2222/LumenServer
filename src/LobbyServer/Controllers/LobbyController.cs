using CommonLib;
using CommonLib.Protocols;
using CommonLib.Protocols.LobbyServer;
using DBMediator.Contexts;
using DBMediator.Models.AccountDB;
using Microsoft.AspNetCore.Mvc;
using ServerCommon;

namespace LobbyServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : LunarController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Req_Login reqParam)
        {
            Res_Login resultParam = new Res_Login();
            using (DbContextAccount accountDB = new DbContextAccount())
            {
                accountDB.BeginTransaction();
                
                RESPONSE_CODE rt = RESPONSE_CODE.CRITICAL;
                switch ((PLATFORM_TYPE)reqParam.PlatformType)
                {
                    default: 
                        throw new Exception("LogIn N/A CredentialType");
                    case PLATFORM_TYPE.GUEST:
                        rt = ProcessGuestLogIn(accountDB, reqParam, resultParam);
                        break;
                    case PLATFORM_TYPE.GOOLEPLAY:
                        rt = ProcessGooglePlayLogin(accountDB, reqParam, resultParam);
                        break;
                    case PLATFORM_TYPE.APPLE:
                        rt = ProcessGamecenterLogin(accountDB, reqParam, resultParam);
                        break;
                }

                resultParam.SessionKey = string.Format("{0}@@{1}@@{2}", resultParam.AccountIdx, DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff"), Guid.NewGuid().ToString());
                accountDB.RefreshSessionKey(resultParam.AccountIdx, resultParam.SessionKey);

                accountDB.CommitTransaction();
            }

            return SendSuccess(resultParam);
        }

        private RESPONSE_CODE ProcessGuestLogIn(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_Guest cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_Guest>(reqParam.PlatformId)!;

            return ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }

        private RESPONSE_CODE ProcessGooglePlayLogin(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_GooglePlay cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_GooglePlay>(reqParam.PlatformId)!;

            return ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }
        private RESPONSE_CODE ProcessGamecenterLogin(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_GameCenter cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_GameCenter>(reqParam.PlatformId)!;

            return ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }

        protected RESPONSE_CODE ProcessCommonLogIn(DbContextAccount dbContext, Req_Login reqParam, Res_Login resultParam, string credentialId)
        {
            bool isNewJoin = false;
            var acrt = dbContext.CommonLogin((int)reqParam.PlatformType, (int)reqParam.MktType, credentialId, ref isNewJoin);

            if (null == acrt)
            {
                return RESPONSE_CODE.CRITICAL;
            }

            var shardInfo = GetShardDB(dbContext, acrt.Idx);
            if (null == shardInfo)
            {
                return RESPONSE_CODE.CRITICAL;
            }

            var restric = dbContext.GetAccountRestric(acrt.Idx);
            if (null != restric)
            {
                resultParam.SanctionInfo = new AccountSanction();
                resultParam.SanctionInfo.AccIdx = acrt.Idx;
                resultParam.SanctionInfo.SanctionType = (SANCTION_TYPE)restric.ResticType;
                resultParam.SanctionInfo.SanctionMessageKey = restric.Stringkey;
                resultParam.SanctionInfo.SanctionEndDate = restric.Enddate;
                return RESPONSE_CODE.SUCCESS;
            }

            if (isNewJoin)
            {
                // 신규계정
                // 닉네임을 자동으로 생성시킨다.
                string nickname = string.Format("#{0}", acrt.Idx);
                dbContext.AddAccountProperties(acrt.Idx, nickname, 1);
            }

            resultParam.AccountIdx = acrt.Idx;
            return RESPONSE_CODE.SUCCESS;
        }



        protected ConfigSharddb GetShardDB(DbContextAccount context, long accountIdx)
        {
            var shardlink = context.GetShardLink(accountIdx);
            if (null != shardlink) return DbConfig.Instance.Get(shardlink.ShardUid);

            var choose = DbConfig.Instance.Determine(accountIdx);
            int rt = context.AddShardLink(accountIdx, choose.Uid);
            if (0 != rt) return null!;

            return choose;
        }
    }
}
