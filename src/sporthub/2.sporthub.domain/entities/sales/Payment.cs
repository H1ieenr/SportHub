
namespace SportHub.Domain;

public class Payment : AuditableEntity
{
    public long order_id { get; set; }
    public PaymentMethod method { get; set; }
    public decimal amount { get; set; }
    public PaymentStatus status { get; set; } = PaymentStatus.Unpaid;
    public string? transaction_code { get; set; }
    public DateTimeOffset? paid_at { get; set; }
    public string? raw_response { get; set; }

    public Order order { get; set; } = null!;
}
