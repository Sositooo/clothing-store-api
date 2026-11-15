namespace ClothingStore.Api.Models;

/// <summary>Размер конкретного товара и его остаток на складе.</summary>
public class ClothingSize
{
    public int Id { get; set; }

    /// <summary>Буквенное обозначение: S, M, L, XL...</summary>
    public string Letter { get; set; } = string.Empty;

    /// <summary>Числовое обозначение: 44, 46, 48...</summary>
    public int Number { get; set; }

    /// <summary>Количество товара этого размера в наличии.</summary>
    public int Quantity { get; set; }

    public int ClothingId { get; set; }
    public Clothing Clothing { get; set; } = null!;
}
