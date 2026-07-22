using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

public class ValidateZuIdAttribute : ActionFilterAttribute
{
    private const string HeaderKey = "zuId"; // Header cố định

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Lấy zuId từ Claims trong AccessToken
        var userZuId = context.HttpContext.User.FindFirst("zuId")?.Value;

        if (string.IsNullOrEmpty(userZuId))
        {
            context.Result = new ForbidResult(); // Không có zuId trong claim → từ chối truy cập
            return;
        }

        // Lấy zuId từ Header cố định
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var headerZuId))
        {
            context.Result = new BadRequestObjectResult($"Thiếu header {HeaderKey}");
            return;
        }

        // So sánh với giá trị trong AccessToken
        if (userZuId != headerZuId.ToString())
        {
            context.Result = new UnauthorizedResult(); // Nếu không khớp, từ chối truy cập
        }
    }
}
