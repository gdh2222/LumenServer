using CommonLib;
using CommonLib.Protocols;
using CommonLib.Protocols.LobbyServer;
using DBMediator.Contexts;
using DBMediator.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ServerCommon;

namespace LobbyServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : LunarController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly DbContextFactory _factory;

        public AuthController(ILogger<AuthController> logger, DbContextFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Req_Login reqParam)
        {
            Res_Login resultParam = new Res_Login();
            using (DbContextAccount accountDB = _factory.Create<DbContextAccount>())
            {
                using (var transaction = accountDB.Database.BeginTransaction())
                {

                    RESPONSE_CODE rt = RESPONSE_CODE.CRITICAL;
                    switch ((PLATFORM_TYPE)reqParam.PlatformType)
                    {
                        default:
                            throw new Exception("LogIn N/A CredentialType");
                        case PLATFORM_TYPE.GUEST:
                            rt = await ProcessGuestLogIn(accountDB, reqParam, resultParam);
                            break;
                        case PLATFORM_TYPE.GOOLEPLAY:
                            rt = await ProcessGooglePlayLogin(accountDB, reqParam, resultParam);
                            break;
                        case PLATFORM_TYPE.APPLE:
                            rt = await ProcessGamecenterLogin(accountDB, reqParam, resultParam);
                            break;
                    }

                    resultParam.SessionKey = string.Format("{0}@@{1}@@{2}", resultParam.AccountIdx, DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff"), Guid.NewGuid().ToString());

                    var sessionData = await accountDB.Accountsessionkeys.FirstOrDefaultAsync(r => r.Accidx == reqParam.AccIdx);
                    if(null == sessionData)
                    {
                        sessionData = new DBMediator.Models.Account.Accountsessionkey();
                        sessionData.Accidx = reqParam.AccIdx;
                        sessionData.Skey = resultParam.SessionKey;
                        await accountDB.AddAsync(sessionData);
                    }
                    else
                    {
                        sessionData.Skey = resultParam.SessionKey;
                        accountDB.Update(sessionData);
                    }

                    await accountDB.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }

            return SendSuccess(resultParam);
        }

        private async Task<RESPONSE_CODE> ProcessGuestLogIn(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_Guest cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_Guest>(reqParam.PlatformId)!;

            return await ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }

        private async Task<RESPONSE_CODE> ProcessGooglePlayLogin(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_GooglePlay cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_GooglePlay>(reqParam.PlatformId)!;

            return await ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }
        private async Task<RESPONSE_CODE> ProcessGamecenterLogin(DbContextAccount context, Req_Login reqParam, Res_Login resultParam)
        {
            Credential_GameCenter cdparam = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential_GameCenter>(reqParam.PlatformId)!;

            return await ProcessCommonLogIn(context, reqParam, resultParam, cdparam.AccountToken);
        }

        protected async Task<RESPONSE_CODE> ProcessCommonLogIn(DbContextAccount dbContext, Req_Login reqParam, Res_Login resultParam, string credentialId)
        {
            bool isNewJoin = false;
            //var acrt = dbContext.CommonLogin((int)reqParam.PlatformType, (int)reqParam.MktType, credentialId, ref isNewJoin);

            //if (null == acrt)
            //{
            //    return RESPONSE_CODE.CRITICAL;
            //}

            DateTime nowDbTime = await dbContext.Now();


            var credentialData = await dbContext.Accountcredentiallinks.FirstOrDefaultAsync(r => r.Cdtype == (int)reqParam.PlatformType && r.Acctoken == credentialId);
            Accountmember member;
            if (null == credentialData)
            {
                isNewJoin = true;

                member = new Accountmember();
                member.Stateflag = 0;
                member.CreateDt = nowDbTime;
                member.LastLoginDt = nowDbTime;
                member.Mkt = (int)reqParam.MktType;
                await dbContext.Accountmembers.AddAsync(member);

                // member 의 auto increment 값을 가져오기 위해서 savechange 한번호출
                await dbContext.SaveChangesAsync();

                credentialData = new Accountcredentiallink();
                credentialData.Accidx = member.Idx;
                credentialData.Cdtype = (int)reqParam.PlatformType;
                credentialData.Acctoken = credentialId;
                credentialData.RecentDt = nowDbTime;
                credentialData.CreationDt = nowDbTime;
                await dbContext.Accountcredentiallinks.AddAsync(credentialData);
            }
            else
            {
                member = await dbContext.Accountmembers.FirstOrDefaultAsync(r => r.Idx == credentialData.Accidx) ?? throw new InvalidOperationException("Not found, accountmember data.");
                member.LastLoginDt = nowDbTime;
                dbContext.Update(member);


                credentialData.RecentDt = nowDbTime;
                dbContext.Update(credentialData);
            }


            var restric = await dbContext.Accountrestrictions.FirstOrDefaultAsync(r => r.Accidx == member.Idx);
            if(null != restric)
            {
                resultParam.SanctionInfo = new AccountSanction();
                resultParam.SanctionInfo.AccIdx = member.Idx;
                resultParam.SanctionInfo.SanctionType = (SANCTION_TYPE)restric.ResticType;
                resultParam.SanctionInfo.SanctionMessageKey = restric.Stringkey;
                resultParam.SanctionInfo.SanctionEndDate = restric.Enddate;
                return RESPONSE_CODE.SUCCESS;
            }

            var shardData = await dbContext.Accountsharddblinks.FirstOrDefaultAsync(r => r.Accidx == member.Idx);
            if(null != shardData)
            {

            }


            if (isNewJoin)
            {
                Accountproperty newAccount = new Accountproperty();
                string nickname = string.Format("#{0}", member.Idx);

                newAccount.Accidx = member.Idx;
                newAccount.Nickname = nickname;
                newAccount.ChangeableNick = 1;
                newAccount.RecentNickDt = nowDbTime;
                dbContext.Accountproperties.Add(newAccount);
            }

            resultParam.AccountIdx = member.Idx;
            return RESPONSE_CODE.SUCCESS;
        }



        //protected ConfigSharddb GetShardDB(DbContextAccount context, long accountIdx)
        //{
        //    var shardlink = context.GetShardLink(accountIdx);
        //    if (null != shardlink) return DbConfig.Instance.Get(shardlink.ShardUid);

        //    var choose = DbConfig.Instance.Determine(accountIdx);
        //    int rt = context.AddShardLink(accountIdx, choose.Uid);
        //    if (0 != rt) return null!;

        //    return choose;
        //}
    }
}
