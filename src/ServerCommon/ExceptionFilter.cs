using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServerCommon
{
    public class LunarExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            // TODO : Production 환경에서는 Exception Message 가 디테일하게 가지 안도록 처리
            // 보안상 Exception Message 가 디테일하게 전달될경우 위험

            var exception = context.Exception;

            // ProblemDetails 객체 생성
            // 표준에러처리 클래스
            var problemDetails = new ProblemDetails
            {
                Status = 500,  // 상태 코드 설정 (예: 500 Internal Server Error)
                Title = "An unexpected error occurred",
                Detail = exception.Message,  // 예외 메시지
                Instance = context.HttpContext.Request.Path // 요청 경로
            };

            // 예외 처리 후 응답을 설정
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = 500 // 500 상태 코드 반환
            };

            // 예외 처리 완료 후 기본 처리 방지
            context.ExceptionHandled = true;
        }

    }
}
