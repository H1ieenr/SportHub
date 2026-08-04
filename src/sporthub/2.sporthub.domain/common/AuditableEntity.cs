namespace SportHub.Domain;

public abstract class AuditableEntity
{
    public long id { get; set; } = 0;
    public DateTimeOffset created_date { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? updated_date { get; set; }
    public DateTimeOffset? deleted_date { get; set; }

    public bool is_deleted => deleted_date.HasValue;
}
