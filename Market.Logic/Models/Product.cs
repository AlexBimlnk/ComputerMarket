namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Продукт.
/// </summary>
public sealed class Product: IEquatable<Product>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Product"/>.
    /// </summary>
    /// <param name="item" xml:lang = "ru">Описание продукта.</param>
    /// <param name="provider" xml:lang = "ru">Поставщик продукта.</param>
    /// <param name="price" xml:lang = "ru">Цена назначенная поставщиком.</param>
    /// <param name="quantity" xml:lang = "ru">Колличество продукта.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="item"/> или <paramref name="price"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Если <paramref name="quantity"/> меньше 0.
    /// </exception>
    public Product(Item item, Provider provider, Price price, int quantity)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Price = price;
        Provider = provider ?? throw new ArgumentNullException(nameof(item));

        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = quantity;
    }

    private Price Price { get; }

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    public Item Item { get; }

    /// <summary xml:lang = "ru">
    /// Цена назначаенная поставщиком.
    /// </summary>
    public decimal ProviderCost => Price.Value;

    /// <summary xml:lang = "ru">
    /// Итоговая цена продукта.
    /// </summary>
    public decimal FinalCost => Price.Value * Provider.Margin.Value;

    /// <summary xml:lang = "ru">
    /// Поставщик продукта.
    /// </summary>
    public Provider Provider { get; }

    /// <summary xml:lang = "ru">
    /// Количестов продукта.
    /// </summary>
    public int Quantity { get; }

    /// <inheritdoc/>
    public override int GetHashCode() =>
        HashCode.Combine(Provider, Item);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Product product && Equals(product);

    /// <inheritdoc/>
    public bool Equals(Product? other) =>
        Provider.Equals(other?.Provider) &&
        Item.Equals(other?.Item);
}
