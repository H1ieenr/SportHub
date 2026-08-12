namespace sporthub.domain;

public abstract class AuditableEntity
{
    public long id { get; set; } = 0;
    public DateTimeOffset created_date { get; set; } = DateTimeOffset.UtcNow;
    public long created_by { get; set; } = 0;
    public DateTimeOffset? updated_date { get; set; }
    public long updated_by { get; set; } = 0;
    public DateTimeOffset? deleted_date { get; set; }
    public long deleted_by { get; set; } = 0;

    public bool is_deleted => deleted_date.HasValue;
}
