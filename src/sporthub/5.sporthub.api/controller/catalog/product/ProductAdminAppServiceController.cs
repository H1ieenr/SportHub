using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/product")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class ProductAdminAppServiceController  : ApiControllerBase
    {
        private readonly IProductAppService _productAppService;

        public ProductAdminAppServiceController(IProductAppService productAppService, IHttpContextAccessor httpContextAccessor)
        {
            _productAppService = productAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateProductRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateProductRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.UpdateAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.DeleteAsync, cancellationToken);
        }
        [HttpPost("feature")]
        public async Task<IActionResult> UpdateFeaturedAsync([FromBody] UpdateFeaturedProductRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.UpdateFeaturedAsync, cancellationToken);
        }
        [HttpGet("view")]
        public async Task<IActionResult> ProductGetByIdAsync([FromQuery] ProductGetByIdRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.ProductGetByIdAsync, cancellationToken);
        }
        [HttpGet("list")]
        public async Task<IActionResult> ProductGetPagedAsync([FromQuery] GetProductPagedRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.ProductGetPagedAsync, cancellationToken);
        }
        [HttpGet("list-nopaging")]
        public async Task<IActionResult> ProductGetNoPagingAsync([FromQuery] GetProductNoPagingRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productAppService.ProductGetNoPagingAsync, cancellationToken);
        }
        
    }
}