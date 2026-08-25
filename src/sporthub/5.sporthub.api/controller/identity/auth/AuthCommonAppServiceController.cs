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
    [Route("api/v1/sporthub/auth/")]
    [Authorize]
    public class AuthCommonAppServiceController : ApiControllerBase
    {
        private readonly IAuthCommonAppService _authCommonAppService;

        public AuthCommonAppServiceController(IAuthCommonAppService authCommonAppService, IHttpContextAccessor httpContextAccessor)
        {
            _authCommonAppService = authCommonAppService;
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _authCommonAppService.LogoutAsync, cancellationToken);
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _authCommonAppService.RefreshTokenAsync, cancellationToken);
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _authCommonAppService.ChangePasswordAsync, cancellationToken);
        }
    }
}