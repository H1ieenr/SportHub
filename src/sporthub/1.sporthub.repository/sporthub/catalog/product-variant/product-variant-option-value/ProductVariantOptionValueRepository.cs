using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{ 
    public class ProductVariantOptionValueRepository : SportHubGenericRepository<ProductVariantOptionValue>, IProductVariantOptionValueRepository
    {
        private readonly SportHubDbContext _context;
        public ProductVariantOptionValueRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<List<ProductVariantOptionValue>> GetByVariantIdAsync(long productVariantId, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariantOptionValues
                .AsNoTracking()
                .Include(x => x.option_definition)
                .Include(x => x.option_value)
                .Where(x => x.product_variant_id == productVariantId)
                .ToListAsync(cancellationToken);
        }
        public Task<List<ProductVariantOptionValue>> GetTrackedByVariantIdAsync(long productVariantId, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariantOptionValues
                .Where(x => x.product_variant_id == productVariantId)
                .ToListAsync(cancellationToken);
        }
        public Task<bool> ExistsByOptionValueIdAsync(long optionValueId, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariantOptionValues.AnyAsync(x => x.option_value_id == optionValueId, cancellationToken);
        }
        public Task CreateAsync(ProductVariantOptionValue entity, CancellationToken cancellationToken = default) => AddAsync(entity);
        public Task CreateBatchAsync(List<ProductVariantOptionValue> entities, CancellationToken cancellationToken = default) => AddRangeAsync(entities);
        public Task UpdateAsync(ProductVariantOptionValue entity, CancellationToken cancellationToken = default)
        {
            Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteBatchAsync(List<ProductVariantOptionValue> entities, CancellationToken cancellationToken = default)
        {
            DeleteRange(entities);
            return Task.CompletedTask;
        }
    }
}
