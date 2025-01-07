using CommonLib;
using CommonLib.Protocols.LobbyServer;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("[controller]/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public ActionResult<Res_Login> Login([FromBody] Req_Login ReqParam)
        {

            // 1. Check login id
            // TODO : 플랫폼에따른 id 체크 
            if( false == CheckPlatformID(ReqParam.PlatformType, ReqParam.PlatformId))
            {
                // TODO : response errorcode
                return NoContent();
            }

            // accountidx -> account db auto incrementid
            // database check
            // platform id 에 연동된 accountidx 가 존재하는지 확인
            // 존재하지 않는다면 신규 계정


            // gamedatabase 할당
            // gamedatabase account 생성
            // 게임 기획에따라 여기서 닉네임까지 모두 생성해도 무방


            Res_Login response = new Res_Login();
            response.AccountIdx = 0;

            return response;
        }




        public bool CheckPlatformID(PLATFORM_TYPE platformType, string platformId)
        {


            return true;
        }
    }
}
