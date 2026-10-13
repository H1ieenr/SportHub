using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/option-value")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class OptionValueAdminAppServiceController : ApiControllerBase
    {
        private readonly IOptionValueAppService _optionValueAppService;

        public OptionValueAdminAppServiceController(IOptionValueAppService optionValueAppService, IHttpContextAccessor httpContextAccessor)
        {
            _optionValueAppService = optionValueAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateValueAsync([FromBody] CreateOptionValueRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionValueAppService.CreateValueAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateValueAsync([FromBody] UpdateOptionValueRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionValueAppService.UpdateValueAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteValueAsync([FromBody] DeleteOptionValueRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionValueAppService.DeleteValueAsync, cancellationToken);
        }

    }
}