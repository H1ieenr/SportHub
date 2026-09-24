using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/helper")]
    [Authorize]
    public class HelperAppServiceController : ApiControllerBase
    {
        private readonly IEnumHelperService _enumHelperService;

        public HelperAppServiceController(IEnumHelperService enumHelperService, IHttpContextAccessor httpContextAccessor)
        {
            _enumHelperService = enumHelperService;
        }

        [HttpGet("enums")]
        public async Task<IActionResult> GetEnum([FromQuery] EnumRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _enumHelperService.GetEnum, cancellationToken);
        }
    }
}