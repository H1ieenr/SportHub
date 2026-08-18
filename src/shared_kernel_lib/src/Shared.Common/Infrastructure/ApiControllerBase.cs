using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//using MediaR;

namespace Shared.Common
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        //private ISender? _sender;
        //protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected long user_id
        {
            get
            {
                var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("id")
                    ?? User.FindFirstValue(ClaimTypes.Sid);

                return long.TryParse(raw, out var id) ? id : 0;
            }
        }

        protected string? email => User.FindFirst(ClaimTypes.Email)?.Value;
        protected string? user_name => User.Identity?.Name;

        protected void BindRequestContext<T>(T model) where T : BaseRequestDTO
        {
            model.user_id = user_id;
        }

        protected IActionResult ProcessResult<T>(OperationResult<T> result)
        {
            if (result.is_success)
            {
                return Ok(result);
            }

            return result.code switch
            {
                "UNAUTHORIZED" or "INVALID_CREDENTIALS" =>
                    Unauthorized(result),

                "FORBIDDEN" =>
                    Forbid(),

                "NOT_FOUND" =>
                    NotFound(result),

                "CONFLICT" =>
                    Conflict(result),

                "VALIDATION_ERROR" =>
                    BadRequest(result),

                _ =>
                    BadRequest(result)
            };
        }
        protected async Task<IActionResult> HandleAsync<TRequest, TResult>(
            TRequest model,
            Func<TRequest, CancellationToken, Task<OperationResult<TResult>>> handler,
            CancellationToken cancellationToken)
            where TRequest : BaseRequestDTO
        {
            BindRequestContext(model);
            var result = await handler(model, cancellationToken);
            return ProcessResult(result);
        }
    }
}