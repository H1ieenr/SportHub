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
        
        protected string user_id
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("id")
                    ?? User.FindFirstValue(ClaimTypes.Sid)
                    ?? string.Empty;
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
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}