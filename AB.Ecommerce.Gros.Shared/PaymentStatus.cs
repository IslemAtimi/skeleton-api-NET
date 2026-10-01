namespace AB.Ecommerce.Gros;


public enum PaymentStatus
{
    None = 0,
    Payed = 1,
    Partial = 2,
    Refunded=3
}

public enum PaymentMethod
{
    PayOnDelivery = 1,
    BankCheck = 2
}