using System;
using System.Collections.Generic;
using System.Text;

namespace sporthub.domain
{
    public interface IProductVariantOptionValueRepository
    {
        Task<List<ProductVariantOptionValue>> GetByVariantIdAsync(long productVariantId, CancellationToken cancellationToken = default);
        Task<List<ProductVariantOptionValue>> GetTrackedByVariantIdAsync(long productVariantId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByOptionValueIdAsync(long optionValueId, CancellationToken cancellationToken = default);
        Task CreateAsync(ProductVariantOptionValue entity, CancellationToken cancellationToken = default);
        Task CreateBatchAsync(List<ProductVariantOptionValue> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProductVariantOptionValue entity, CancellationToken cancellationToken = default);
        Task DeleteBatchAsync(List<ProductVariantOptionValue> entities, CancellationToken cancellationToken = default);
    }
}
