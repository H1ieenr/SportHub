using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;
namespace sporthub.api
{
    [ApiController]
    [Route("api/v1/sporthub/auth")]
    public class AuthAppServiceController : ApiControllerBase
    {
        private readonly IAuthAppService _authAppService;

        public AuthAppServiceController(IAuthAppService authAppService,IHttpContextAccessor httpContextAccessor)
        {
            _authAppService = authAppService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
        {
            var result = await _authAppService.LoginAsync(model, cancellationToken);
            return ProcessResult(result);
        }
    }
}