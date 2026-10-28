using AutoMapper;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.app.contracts.sporthub.inventory;
using sporthub.domain;

namespace sporthub.app
{
    public class InventoryAppService : IInventoryAppService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryAppService(
            IInventoryItemRepository inventoryItemRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IProductVariantRepository productVariantRepository,
            ISportHubUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _productVariantRepository = productVariantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<InventoryItemDTO>> InventoryGetByVariantIdAsync(
            InventoryGetByVariantIdRequestDTO model, CancellationToken cancellationToken = default)
            => OperationResult<InventoryItemDTO>.Success(await LoadDtoAsync(model.product_variant_id, cancellationToken));

        public async Task<OperationResult<PagedResult<InventoryItemDTO>>> InventoryGetPagedAsync(
            GetInventoryPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _inventoryItemRepository.InventoryItemGetPagedAsync(
                model.page_number, model.page_size, model.search_text, model.product_id,
                model.low_stock, model.sort_by, model.sort_dir, cancellationToken);

            var paged = new PagedResult<InventoryItem>(items, total, model.page_number, model.page_size);
            return OperationResult<PagedResult<InventoryItemDTO>>.Success(paged.MapTo(_mapper.Map<InventoryItemDTO>));
        }

        public async Task<OperationResult<PagedResult<InventoryTransactionDTO>>> InventoryTransactionGetPagedAsync(
            GetInventoryTransactionsPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _inventoryTransactionRepository.InventoryTransactionGetPagedAsync(
                model.page_number, model.page_size, model.product_variant_id,
                model.type, model.from_date, model.to_date, cancellationToken);

            var paged = new PagedResult<InventoryTransaction>(items, total, model.page_number, model.page_size);
            return OperationResult<PagedResult<InventoryTransactionDTO>>.Success(paged.MapTo(_mapper.Map<InventoryTransactionDTO>));
        }

        public async Task<OperationResult<InventoryItemDTO>> ChangeStockAsync(
            ChangeStockRequestDTO model, CancellationToken cancellationToken = default)
        {
            var allowed = new[] { InventoryTransactionType.Receive,
            InventoryTransactionType.AdjustIncrease, InventoryTransactionType.AdjustDecrease };
            if (!allowed.Contains(model.type))
                throw new BadRequestException("Loại giao dịch không hợp lệ.");
            if (model.quantity <= 0 || model.quantity > 1_000_000)
                throw new BadRequestException("Số lượng phải từ 1 đến 1.000.000.");
            if (model.type != InventoryTransactionType.Receive && string.IsNullOrWhiteSpace(model.note))
                throw new BadRequestException("Điều chỉnh tồn kho cần ghi chú lý do.");

            var item = await GetOrCreateTrackedAsync(model.product_variant_id, cancellationToken);

            if (model.type == InventoryTransactionType.AdjustDecrease) item.Decrease(model.quantity);
            else item.Increase(model.quantity);

            await _inventoryTransactionRepository.CreateAsync(
                InventoryTransaction.Create(item.id, model.type, model.quantity, "Manual", null, model.note?.Trim()),
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);   // số tồn và giao dịch lưu cùng một lần

            return OperationResult<InventoryItemDTO>.Success(await LoadDtoAsync(model.product_variant_id, cancellationToken));
        }

        public async Task<OperationResult<InventoryItemDTO>> UpdateReorderLevelAsync(
            UpdateReorderLevelRequestDTO model, CancellationToken cancellationToken = default)
        {
            var item = await GetOrCreateTrackedAsync(model.product_variant_id, cancellationToken);
            item.SetReorderLevel(model.reorder_level);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<InventoryItemDTO>.Success(await LoadDtoAsync(model.product_variant_id, cancellationToken));
        }

        // variant cũ chưa có dòng tồn kho thì tạo luôn (số lượng 0)
        private async Task<InventoryItem> GetOrCreateTrackedAsync(long variantId, CancellationToken cancellationToken)
        {
            var item = await _inventoryItemRepository.InventoryItemGetTrackedByVariantIdAsync(variantId, cancellationToken);
            if (item is not null) return item;

            _ = await _productVariantRepository.ProductVariantGetByIdAsync(variantId, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy biến thể.");

            item = InventoryItem.Create(variantId);
            await _inventoryItemRepository.CreateAsync(item, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);   // cần item.id để ghi giao dịch
            return item;
        }

        private async Task<InventoryItemDTO> LoadDtoAsync(long variantId, CancellationToken ct)
        {
            var item = await _inventoryItemRepository.InventoryItemGetByVariantIdAsync(variantId, ct)
                ?? throw new NotFoundException("Biến thể chưa có tồn kho.");
            return _mapper.Map<InventoryItemDTO>(item);
        }
    }
}
