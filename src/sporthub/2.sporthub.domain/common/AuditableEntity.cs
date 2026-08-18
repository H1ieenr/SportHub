namespace sporthub.domain;

public abstract class AuditableEntity
{
    public long id { get; protected set; } = 0;
    public DateTime created_date { get; protected set; } = DateTime.Now;
    public long created_by { get; protected set; } = 0;
    public DateTime? updated_date { get; protected set; }
    public long updated_by { get; protected set; } = 0;
    public DateTime? deleted_date { get; protected set; }
    public long deleted_by { get; protected set; } = 0;

    public bool is_deleted => deleted_date.HasValue;
    public void SetCreatedInfo(long userId, DateTime date)
    {
        created_by = userId;
        created_date = date;
    }

    public void SetUpdatedInfo(long userId, DateTime date)
    {
        updated_by = userId;
        updated_date = date;
    }

    public void SetDeletedInfo(long userId, DateTime date)
    {
        deleted_by = userId;
        deleted_date = date;
    }
}
