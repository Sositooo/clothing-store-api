namespace ClothingStore.Api.Models;

/// <summary>Тип одежды.</summary>
public enum ClothingType
{
    TShirt,
    Longsleeve,
    Hoodie
}

/// <summary>Статус заказа.</summary>
public enum OrderStatus
{
    Created,     // создан
    Paid,        // оплачен
    Shipped,     // отправлен
    Delivered,   // доставлен
    Cancelled    // отменён
}

/// <summary>Роль пользователя.</summary>
public enum UserRole
{
    User,
    Manager
}
