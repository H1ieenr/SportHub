using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using sporthub.app.contracts;
using sporthub.app.contracts.sporthub.inventory;

namespace sporthub.api
{
    [Route("api/v1/sporthub/admin/inventory")]
    [Authorize(Policy = AppPolicies.AdminAndStaff)]
    public class InventoryAdminAppServiceController : ApiControllerBase
    {
        private readonly IInventoryAppService _inventoryAppService;

        public InventoryAdminAppServiceController(IInventoryAppService inventoryAppService, IHttpContextAccessor httpContextAccessor)
        {
            _inventoryAppService = inventoryAppService;
        }

        [HttpGet("get-by-variant")]
        public async Task<IActionResult> GetByVariant([FromQuery] InventoryGetByVariantIdRequestDTO model, CancellationToken cancellationToken)
        {
             return await HandleAsync(model, _inventoryAppService.InventoryGetByVariantIdAsync, cancellationToken);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] GetInventoryPagedRequestDTO model, CancellationToken cancellationToken)
        {
             return await HandleAsync(model, _inventoryAppService.InventoryGetPagedAsync, cancellationToken);
        }

        [HttpGet("list-transactions")]
        public async Task<IActionResult> Transactions([FromQuery] GetInventoryTransactionsPagedRequestDTO model, CancellationToken cancellationToken)
        {
             return await HandleAsync(model, _inventoryAppService.InventoryTransactionGetPagedAsync, cancellationToken);
        }

        [HttpPost("change-stock")]
        public async Task<IActionResult> ChangeStock([FromBody] ChangeStockRequestDTO model, CancellationToken cancellationToken)
        {
             return await HandleAsync(model, _inventoryAppService.ChangeStockAsync, cancellationToken);
        }

        [HttpPost("reorder-level")]
        public async Task<IActionResult> UpdateReorderLevel([FromBody] UpdateReorderLevelRequestDTO model, CancellationToken cancellationToken)
        {
             return await HandleAsync(model, _inventoryAppService.UpdateReorderLevelAsync, cancellationToken);
        }
    }
}
