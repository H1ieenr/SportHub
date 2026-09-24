using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using sporthub.app.contracts;
using Microsoft.AspNetCore.Http;
using Shared.Common;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/product-image")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class ProductImageAdminAppServiceController : ApiControllerBase
    {
        private readonly IProductImageAppService _productImageAppService;

        public ProductImageAdminAppServiceController(IProductImageAppService productImageAppService, IHttpContextAccessor httpContextAccessor)
        {
            _productImageAppService = productImageAppService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateProductImageRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.CreateAsync, cancellationToken);
        }
        [HttpPost("create-batch")]
        public async Task<IActionResult> CreateBatchAsync([FromForm] CreateBatchProductImageRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.CreateBatchAsync, cancellationToken);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteProductImageRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.DeleteAsync, cancellationToken);
        }
        [HttpPost("update-primary")]
        public async Task<IActionResult> UpdatePrimaryProductImageAsync([FromBody] UpdatePrimaryProductImageRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.UpdatePrimaryProductImageAsync, cancellationToken);
        }
        [HttpPost("update-display-order")]
        public async Task<IActionResult> UpdateDisplayOrderProductImageAsync([FromBody] UpdateDisplayOrderProductImageRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.UpdateDisplayOrderProductImageAsync, cancellationToken);
        }
        [HttpGet("list-nopaging")]
        public async Task<IActionResult> ProductImageGetNoPagingAsync([FromQuery] GetProductImageNoPagingRequestDTO model, CancellationToken cancellationToken)
        {
            return await HandleAsync(model, _productImageAppService.ProductImageGetNoPagingAsync, cancellationToken);
        }
    }
}