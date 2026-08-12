
namespace sporthub.domain;

public class Order : AuditableEntity
{
    public string order_code { get; set; } = "";
    public long user_id { get; set; }
    public string customer_name { get; set; } = "";
    public string customer_phone { get; set; } = "";
    public string? shipping_address { get; set; }
    public decimal sub_total { get; set; }
    public decimal discount_amount { get; set; }
    public decimal shipping_fee { get; set; }
    public decimal total_amount { get; set; }
    public OrderStatus status { get; set; } = OrderStatus.Pending;
    public PaymentStatus payment_status { get; set; } = PaymentStatus.Unpaid;
    public PaymentMethod payment_method { get; set; } = PaymentMethod.CashOnDelivery;
    public string? note { get; set; }

    public Users user { get; set; } = null!;
    public ICollection<OrderItem> items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> payments { get; set; } = new List<Payment>();
}
