using Shared.Common;
using sporthub.app.contracts.sporthub.inventory;
using sporthub.domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace sporthub.app.contracts
{
    public interface IInventoryAppService
    {
        Task<OperationResult<InventoryItemDTO>> InventoryGetByVariantIdAsync(InventoryGetByVariantIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<PagedResult<InventoryItemDTO>>> InventoryGetPagedAsync(GetInventoryPagedRequestDTO model, CancellationToken cancellationToken = default);

        Task<OperationResult<PagedResult<InventoryTransactionDTO>>> InventoryTransactionGetPagedAsync(GetInventoryTransactionsPagedRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<InventoryItemDTO>> ChangeStockAsync(ChangeStockRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<InventoryItemDTO>> UpdateReorderLevelAsync(UpdateReorderLevelRequestDTO model, CancellationToken cancellationToken = default);
    }
}
