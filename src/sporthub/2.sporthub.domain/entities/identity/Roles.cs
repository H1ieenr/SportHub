namespace sporthub.domain;
using System;
using Shared.Exceptions;

public class Roles : AuditableEntity
{
    public string name { get; private set; } = "";
    public string? description { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> user_roles => _userRoles;

    #region Static Factory Method
    public static Roles Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Email không được để trống.");

        return new Roles
        {
            name = name,
            description = description,
        };
    }
    #endregion
}
