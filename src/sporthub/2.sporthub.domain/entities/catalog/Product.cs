using System;
using Shared.Exceptions;
using System.Text.Json;
namespace sporthub.domain;

public class Product : AuditableEntity
{
    public long category_id { get; private set; }
    public long? brand_id { get; private set; }
    public string name { get; private set; } = "";
    public string slug { get; private set; } = "";
    public string? description { get; private set; } = "";
    public decimal base_price { get; private set; }
    public ProductStatus status { get; private set; } = ProductStatus.Draft;
    public bool is_featured { get; private set; }

    public Category category { get; private set; } = null!;
    public Brand? brand { get; private set; }

    private readonly List<ProductVariant> _variants = new();
    public IReadOnlyCollection<ProductVariant> variants => _variants;
    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> images => _images;

    private Product() { }

    public static Product Create(long categoryId, string name, string slug, decimal basePrice,
        long? brandId = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên sản phẩm không được để trống.");
        if (basePrice < 0)
            throw new ValidationException("Giá sản phẩm không hợp lệ.");

        return new Product
        {
            category_id = categoryId,
            brand_id = brandId,
            name = name.Trim(),
            slug = slug.Trim().ToLowerInvariant(),
            description = description,
            base_price = basePrice,
            status = ProductStatus.Draft
        };
    }

    public void Update(long categoryId, string name, string slug, decimal basePrice,
        long? brandId, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên sản phẩm không được để trống.");
        if (basePrice < 0)
            throw new ValidationException("Giá sản phẩm không hợp lệ.");

        category_id = categoryId;
        brand_id = brandId;
        this.name = name.Trim();
        this.slug = slug.Trim().ToLowerInvariant();
        this.description = description;
        base_price = basePrice;
    }

    public void ChangeStatus(ProductStatus newStatus) => status = newStatus;
    public void SetFeatured(bool featured) => is_featured = featured;

    public ProductVariant AddVariant(string sku, string name, decimal costPrice, decimal salePrice,
        JsonElement? valueJson = null)
    {
        var variant = ProductVariant.Create(id, sku, name, costPrice, salePrice, valueJson);
        _variants.Add(variant);
        return variant;
    }

    public void RemoveVariant(ProductVariant variant) => _variants.Remove(variant);

    public ProductImage AddImage(string imageUrl, int displayOrder = 0, bool isPrimary = false,
        long? productVariantId = null)
    {
        var image = ProductImage.Create(id, imageUrl, displayOrder, isPrimary, productVariantId);
        _images.Add(image);
        return image;
    }

    public void RemoveImage(ProductImage image) => _images.Remove(image);
}
