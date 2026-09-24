using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.domain
{
    public interface IProductImageRepository
    {
        Task<ProductImage?> ProductImageGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ProductImage?> ProductImageGetByPrimaryAsync(long? product_id, long? product_variant_id, CancellationToken cancellationToken = default);
        Task<List<ProductImage>> ProductImageGetNoPagingAsync(
              long? product_id,
              long? product_variant_id,
              bool? is_primary,
              CancellationToken cancellationToken = default);
        Task CreateAsync(ProductImage productImage, CancellationToken cancellationToken = default);
        Task CreateBatchAsync(List<ProductImage> productImages, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProductImage productImage, CancellationToken cancellationToken = default);
        Task DeleteAsync(ProductImage productImage, CancellationToken cancellationToken = default);
        Task DeleteBatchAsycn(List<ProductImage> productImages, CancellationToken cancellationToken = default);
    }
}