namespace SportHub.Domain;

public enum UserStatus
{
    Active = 1,
    Locked = 2,
    Inactive = 3
}

public enum ProductStatus
{
    Draft = 1,
    Active = 2,
    Inactive = 3,
    Discontinued = 4
}

public enum CartStatus
{
    Active = 1,
    Converted = 2,
    Abandoned = 3
}

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Packing = 3,
    Shipping = 4,
    Completed = 5,
    Cancelled = 6,
    Returned = 7
}

public enum PaymentStatus
{
    Unpaid = 1,
    Paid = 2,
    Failed = 3,
    Refunded = 4
}

public enum PaymentMethod
{
    CashOnDelivery = 1,
    Cash = 2,
    BankTransfer = 3,
    VnPay = 4,
    Momo = 5
}

public enum InventoryTransactionType
{
    Import = 1,
    Sale = 2,
    Return = 3,
    Adjustment = 4,
    CancelOrder = 5
}
