namespace ClothingStore.Api.Models;

/// <summary>Товар (одежда).</summary>
public class Clothing
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    /// <summary>Сколько раз товар был куплен (суммарное количество проданных штук).</summary>
    public int PurchaseCount { get; set; }

    public ClothingType Type { get; set; }

    /// <summary>Доступен ли товар для продажи.</summary>
    public bool IsAvailable { get; set; } = true;

    public List<ClothingSize> Sizes { get; set; } = [];
}
