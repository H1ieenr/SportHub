using Shared.Common;
namespace sporthub.app.contracts
{
    public interface IProductAppService
    {
        Task<OperationResult<ProductDTO>> CreateAsync(CreateProductRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<ProductDTO>> UpdateAsync(UpdateProductRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(DeleteProductRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>>UpdateFeaturedAsync(UpdateFeaturedProductRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<ProductDTO>> ProductGetByIdAsync(ProductGetByIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<PagedResult<ProductDTO>>> ProductGetPagedAsync(GetProductPagedRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<List<ProductDTO>>> ProductGetNoPagingAsync(GetProductNoPagingRequestDTO model, CancellationToken cancellationToken = default);
    }
}