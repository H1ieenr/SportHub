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
    [Route("api/v1/sporthub/admin/auth/")]
    public class AuthAdminAppServiceController : ApiControllerBase
    {
        private readonly IAdminAuthAppService _adminAuthAppService;

        public AuthAdminAppServiceController(IAdminAuthAppService adminAuthAppService, IHttpContextAccessor httpContextAccessor)
        {
            _adminAuthAppService = adminAuthAppService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _adminAuthAppService.LoginAsync, cancellationToken);
        }
    }
}