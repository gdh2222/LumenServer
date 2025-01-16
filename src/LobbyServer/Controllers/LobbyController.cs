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

            return Ok();
        }




        public bool CheckPlatformID(PLATFORM_TYPE platformType, string platformId)
        {


            return true;
        }
    }
}
