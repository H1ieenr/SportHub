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
    [Route("api/v1/sporthub/admin/brand")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class BrandAdminAppServiceController : ApiControllerBase
    {
        private readonly IBrandAppService _brandAppService;

        public BrandAdminAppServiceController(IBrandAppService brandAppService, IHttpContextAccessor httpContextAccessor)
        {
            _brandAppService = brandAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBrandRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateBrandRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.UpdateAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteBrandRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.DeleteAsync, cancellationToken);
        }
        [HttpPost("active")]
        public async Task<IActionResult> UpdateActiveAsync([FromBody] UpdateActiveBrandRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.UpdateActiveAsync, cancellationToken);
        }
        [HttpGet("view")]
        public async Task<IActionResult> BrandGetByIdAsync([FromQuery] BrandGetByIdRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.BrandGetByIdAsync, cancellationToken);
        }
        [HttpGet("list")]
        public async Task<IActionResult> BrandGetPagedAsync([FromQuery] GetBrandsPagedRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _brandAppService.BrandGetPagedAsync, cancellationToken);
        }
    }
}