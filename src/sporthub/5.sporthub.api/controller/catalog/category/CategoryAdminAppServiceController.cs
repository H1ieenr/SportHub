using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/category")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class CategoryAdminAppServiceController : ApiControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;

        public CategoryAdminAppServiceController(ICategoryAppService categoryAppService, IHttpContextAccessor httpContextAccessor)
        {
            _categoryAppService = categoryAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateCategoryRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateCategoryRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.UpdateAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCategoryRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.DeleteAsync, cancellationToken);
        }
        [HttpPost("active")]
        public async Task<IActionResult> UpdateActiveAsync([FromBody] UpdateActiveCategoryRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.UpdateActiveAsync, cancellationToken);
        }
        [HttpGet("view")]
        public async Task<IActionResult> CategoryGetByIdAsync([FromQuery] CategoryGetByIdRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.CategoryGetByIdAsync, cancellationToken);
        }
        [HttpGet("list")]
        public async Task<IActionResult> CategoryGetPagedAsync([FromQuery] GetCategoriesPagedRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.CategoryGetPagedAsync, cancellationToken);
        }
        [HttpGet("list-nopaging")]
        public async Task<IActionResult> CategoryGetNoPagingAsync([FromQuery] GetCategoriesNoPagingRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _categoryAppService.CategoryGetNoPagingAsync, cancellationToken);
        }

    }
}