using CommonLib.Protocols.VersionChecker;
using DBMediator;
using Microsoft.AspNetCore.Mvc;

namespace VersionChecker.Controllers
{
    [ApiController]
    [Route("api/version")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("check")]
        public ActionResult<Res_VersionCheck> Check([FromBody] Req_VersionCheck ReqParam)
        {
            // 1. Check Version
            Version? clientVer;
            Version? serverVer;
            if( false == Version.TryParse(ReqParam.Version, out clientVer))
            {
                // TODO : Send error reason code
                return NoContent();
            }

            string serverVersionString = FakeDB_Service.GetGameVersion((int)ReqParam.OsType);
            if (false == Version.TryParse(serverVersionString, out serverVer))
            {
                // TODO : Send error reason code
                return NoContent();
            }

            if(serverVer.Major > clientVer.Major)
            {
                // TODO : Send error reason code
                // ������������Ʈ
                // ����������Ʈ�ʿ�, �������� �����̵�
                return NoContent();
            }


            // 2. Check Maintenance
            var maintenanceInfo = FakeDB_Service.GetGameVersion((int)ReqParam.OsType);
            if(null != maintenanceInfo)
            {
                // TODO : Send error reason code
                // ������, �ð� �� ���˻����ڵ� ����
                return NoContent();
            }


            // 3. Redirect Gameserver url
            var redirectInfo = FakeDB_Service.GetRedirectInfo((int)ReqParam.OsType, ReqParam.Version);
            if(null != redirectInfo)
            {
                // TODO : Send error reason code
                // �����̷�Ʈ, ���� ���Ӽ������ƴ�, �ٸ������� �����̷�Ʈ
                return NoContent();
            }

            // 4. CDN Url ����
            // Sample Url
            // http://localhost:8080/ANDROID/1.0.0
            string cdnUrl = $"{GlobalConfig.CDNUrl}/{ReqParam.OsType}/{clientVer.Major}.{clientVer.Minor}.{clientVer.Build}";


            // 5. ����
            var res = new Res_VersionCheck();
            return res;
        }


    }
}
