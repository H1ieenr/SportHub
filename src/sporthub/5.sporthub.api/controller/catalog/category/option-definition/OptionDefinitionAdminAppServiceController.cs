using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/option-definition")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class OptionDefinitionAdminAppServiceController : ApiControllerBase
    {
        private readonly IOptionDefinitionAppService _optionDefinitionAppService;

        public OptionDefinitionAdminAppServiceController(IOptionDefinitionAppService optionDefinitionAppService, IHttpContextAccessor httpContextAccessor)
        {
            _optionDefinitionAppService = optionDefinitionAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOptionDefinitionRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionDefinitionAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOptionDefinitionRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionDefinitionAppService.UpdateAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteOptionDefinitionRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionDefinitionAppService.DeleteAsync, cancellationToken);
        }
        [HttpGet("list-nopaging")]
        public async Task<IActionResult> OptionDefinitionGetNoPagingAsync([FromQuery] GetOptionDefinitionsNoPagingRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _optionDefinitionAppService.OptionDefinitionGetNoPagingAsync, cancellationToken);
        }
    }
}