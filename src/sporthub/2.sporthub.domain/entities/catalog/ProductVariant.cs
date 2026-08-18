using System.Text.Json;
using System;
using Shared.Exceptions;

namespace sporthub.domain;

public class ProductVariant : AuditableEntity
{
    public long product_id { get; private set; }
    public string sku { get; private set; } = "";
    public string name { get; private set; } = "";
    public decimal cost_price { get; private set; }
    public decimal sale_price { get; private set; }
    public bool is_active { get; private set; } = true;
    public JsonElement? value_json { get; private set; }

    public Product product { get; private set; } = null!;
    public InventoryItem? inventory { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images;

    private ProductVariant() { }

    public static ProductVariant Create(long productId, string sku, string name,
        decimal costPrice, decimal salePrice, JsonElement? valueJson = null)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ValidationException("SKU không được để trống.");
        if (salePrice < 0 || costPrice < 0)
            throw new ValidationException("Giá không hợp lệ.");

        return new ProductVariant
        {
            product_id = productId,
            sku = sku.Trim().ToUpperInvariant(),
            name = name.Trim(),
            cost_price = costPrice,
            sale_price = salePrice,
            value_json = valueJson
        };
    }

    public void Update(string name, decimal costPrice, decimal salePrice, JsonElement? valueJson)
    {
        if (salePrice < 0 || costPrice < 0)
            throw new ValidationException("Giá không hợp lệ.");

        this.name = name.Trim();
        cost_price = costPrice;
        sale_price = salePrice;
        value_json = valueJson;
    }

    public void Activate() => is_active = true;
    public void Deactivate() => is_active = false;
}
