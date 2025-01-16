
using CommonLib.Protocols;
using Microsoft.AspNetCore.Mvc;

namespace ServerCommon
{
    public class LunarController : ControllerBase
    {

        protected IActionResult SendSuccess(RES_Header obj)
        {
            return Ok(new { 
            
                ErrorCode = (int)RESPONSE_CODE.SUCCESS,
                Data = obj
            });
        }

        protected IActionResult SendError(RESPONSE_CODE rspCode, string message = "")
        {
            return Ok(new
            {
                ErrorCode = (int)rspCode,
                Message = message
            });
        }


    }
}
