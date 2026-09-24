using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class ProductImageRepository : SportHubGenericRepository<ProductImage>, IProductImageRepository
    {
        private readonly SportHubDbContext _context;
        public ProductImageRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<ProductImage?> ProductImageGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<ProductImage?> ProductImageGetByPrimaryAsync(long? product_id, long? product_variant_id, CancellationToken cancellationToken = default)
        {
            return _context.ProductImages.FirstOrDefaultAsync(x => x.product_id == product_id && x.product_variant_id == product_variant_id, cancellationToken);
        }
        public async Task<List<ProductImage>> ProductImageGetNoPagingAsync(
              long? product_id,
              long? product_variant_id,
              bool? is_primary,
              CancellationToken cancellationToken = default)
        {
            var query = _context.ProductImages.AsNoTracking().AsQueryable();

            if (product_id.HasValue)
                query = query.Where(x => x.product.id == product_id.Value);

            if (product_variant_id.HasValue)
                query = query.Where(x => x.product_variant_id == product_variant_id.Value);

            if (is_primary.HasValue)
                query = query.Where(x => x.is_primary == is_primary.Value);

            var items = await query.OrderBy(x => x.display_order).ToListAsync(cancellationToken);

            return items;
        }
        public Task CreateAsync(ProductImage productImage, CancellationToken cancellationToken = default)
        {
            return AddAsync(productImage);
        }
        public Task CreateBatchAsync(List<ProductImage> productImages, CancellationToken cancellationToken = default)
        {
            return AddRangeAsync(productImages);
        }
        public Task UpdateAsync(ProductImage productImage, CancellationToken cancellationToken = default)
        {
            Update(productImage);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(ProductImage productImage, CancellationToken cancellationToken = default)
        {
            Delete(productImage);
            return Task.CompletedTask;
        }
        public Task DeleteBatchAsycn(List<ProductImage> productImages, CancellationToken cancellationToken = default)
        {
            DeleteRange(productImages);
            return Task.CompletedTask;
        }

    }
}