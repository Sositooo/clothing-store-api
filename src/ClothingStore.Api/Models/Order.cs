using System.Text.Json.Serialization;

namespace ClothingStore.Api.Models;

/// <summary>Заказ.</summary>
public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Created;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public List<OrderItem> Items { get; set; } = [];
}

/// <summary>Позиция заказа: какой товар, какого размера и сколько штук.</summary>
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; } = null!;

    public int ClothingSizeId { get; set; }
    public ClothingSize ClothingSize { get; set; } = null!;

    public int Quantity { get; set; }
}
