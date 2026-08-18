using System;
using Shared.Exceptions;
namespace sporthub.domain;

public class ProductImage : AuditableEntity
{
    public long product_id { get; private set; }
    public long? product_variant_id { get; private set; }
    public string image_url { get; private set; } = "";
    public int display_order { get; private set; }
    public bool is_primary { get; private set; }

    public Product product { get; private set; } = null!;
    public ProductVariant? product_variant { get; private set; }

    private ProductImage() { }

    public static ProductImage Create(long productId, string imageUrl, int displayOrder = 0,
        bool isPrimary = false, long? productVariantId = null)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ValidationException("Đường dẫn ảnh không được để trống.");

        return new ProductImage
        {
            product_id = productId,
            product_variant_id = productVariantId,
            image_url = imageUrl,
            display_order = displayOrder,
            is_primary = isPrimary
        };
    }

    public void SetPrimary(bool isPrimary) => this.is_primary = isPrimary;
    public void UpdateOrder(int displayOrder) => this.display_order = displayOrder;
}
