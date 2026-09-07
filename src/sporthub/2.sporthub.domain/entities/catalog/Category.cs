using System;
using Shared.Exceptions;

namespace sporthub.domain;

public class Category : AuditableEntity
{
    public string name { get; private set; } = "";
    public string slug { get; private set; } = "";
    public string? description { get; private set; }
    public string? image_url { get; private set; }
    public string? image_public_id { get; private set; }
    public long? parent_id { get; private set; }
    public int display_order { get; private set; }
    public bool is_active { get; private set; } = true;

    public Category? parent { get; private set; }
    private readonly List<Category> _children = new();
    public IReadOnlyCollection<Category> children => _children;
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> products => _products;

    private Category() { }

    public static Category Create(string name, string slug, long? parentId = null,
        string? description = null, int displayOrder = 0, bool isActive = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên danh mục không được để trống.");

        return new Category
        {
            name = name.Trim(),
            slug = slug.Trim().ToLowerInvariant(),
            parent_id = parentId,
            description = description,
            display_order = displayOrder,
            is_active = isActive
        };
    }

    public void Update(string name, string slug, long? parentId, string? description, int displayOrder, bool isActive = false   )
    {
        if (parentId == id)
            throw new ValidationException("Danh mục không thể là danh mục cha của chính nó.");

        this.name = name.Trim();
        this.slug = slug.Trim().ToLowerInvariant();
        parent_id = parentId;
        this.description = description;
        this.display_order = displayOrder;
        this.is_active = isActive;
    }

    public void UpdateLogo(string? logoUrl, string? logoPublicId)
    {
        image_url = logoUrl;
        image_public_id = logoPublicId;
    }

    public void Activate() => is_active = true;
    public void Deactivate() => is_active = false;
}
