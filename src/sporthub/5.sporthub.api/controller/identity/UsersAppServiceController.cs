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
    [Route("api/v1/sporthub/users")]
    [Authorize]
    public class UsersAppServiceController : ApiControllerBase
    {
        private readonly IUsersAppService _usersAppService;

        public UsersAppServiceController(IUsersAppService usersAppService, IHttpContextAccessor httpContextAccessor)
        {
            _usersAppService = usersAppService;
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] GetByIdAsyncRquestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _usersAppService.GetByIdAsync, cancellationToken);
        }
    }
}