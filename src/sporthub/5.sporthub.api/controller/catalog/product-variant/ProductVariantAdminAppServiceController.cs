using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/product-variant")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class ProductVariantAdminAppServiceController : ApiControllerBase
    {
        private readonly IProductVariantAppService _productVariantAppService;

        public ProductVariantAdminAppServiceController(IProductVariantAppService productVariantAppService, IHttpContextAccessor httpContextAccessor)
        {
            _productVariantAppService = productVariantAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProductVariantRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("create-batch")]
        public async Task<IActionResult> CreateBatchAsync([FromBody] CreateBatchProductVariantRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.CreateBatchAsync, cancellationToken);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProductVariantRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.UpdateAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductVariantRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.DeleteAsync, cancellationToken);
        }
        [HttpPost("active")]
        public async Task<IActionResult> UpdateActiveProductVariantAsync([FromBody] UpdateActiveProductVariantRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.UpdateActiveProductVariantAsync, cancellationToken);
        }
        [HttpGet("view")]
        public async Task<IActionResult> ProductVariantGetByIdAsync([FromQuery] ProductVariantGetByIdRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.ProductVariantGetByIdAsync, cancellationToken);
        }
        [HttpGet("list")]
        public async Task<IActionResult> ProductVariantGetPagedAsync([FromQuery] GetProductVariantPagedRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.ProductVariantGetPagedAsync, cancellationToken);
        }
        [HttpGet("list-nopaging")]
        public async Task<IActionResult> ProductVariantGetNoPagingAsync([FromQuery] GetProductVariantNoPagingRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productVariantAppService.ProductVariantGetNoPagingAsync, cancellationToken);
        }
    }
}