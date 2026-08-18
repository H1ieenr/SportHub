using System;
using Shared.Exceptions;

namespace sporthub.domain;

public class Brand : AuditableEntity
{
    public string name { get; private set; } = "";
    public string slug { get; private set; } = "";
    public string? logo_url { get; private set; } = "";
    public string? description { get; private set; } = "";
    public bool is_active { get; private set; } = true;

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> products => _products;

    private Brand() {} 

    public static Brand Create(string name, string slug, string? logoUrl = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên thương hiệu không được để trống.");
        if (string.IsNullOrWhiteSpace(slug))
            throw new ValidationException("Slug không được để trống.");

        return new Brand
        {
            name = name.Trim(),
            slug = slug.Trim().ToLowerInvariant(),
            logo_url = logoUrl,
            description = description
        };
    }

    public void Update(string name, string slug, string? logoUrl, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên thương hiệu không được để trống.");

        this.name = name.Trim();
        this.slug = slug.Trim().ToLowerInvariant();
        logo_url = logoUrl;
        this.description = description;
    }

    public void Activate() => is_active = true;
    public void Deactivate() => is_active = false;
}
