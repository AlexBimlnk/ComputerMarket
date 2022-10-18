namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Позиция в заказе.
/// </summary>
public sealed class OrderItem: IEquatable<OrderItem>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="OrderItem"/>.
    /// </summary>
    /// <param name="product"  xml:lang = "ru">Товар в позиции заказа.</param>
    /// <param name="quantity"  xml:lang = "ru">Колличесиво товар в позиции.</param>
    /// <exception cref="ArgumentNullException"  xml:lang = "ru">Если <paramref name="product"/> - <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"  xml:lang = "ru"> Если <paramref name="quantity"/> <= 0 или больше колличества продуктов.</exception>
    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        
        if (quantity < 1 || quantity > product.Quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = quantity;
    }

    /// <summary  xml:lang = "ru">
    ///  Продукт.
    /// </summary>
    public Product Product { get; }

    /// <summary  xml:lang = "ru">
    /// Количество продуктов.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary xml:lang = "ru">
    /// Выбран ли продукт.
    /// </summary>
    public bool Selected { get; set; } = true;

    /// <summary xml:lang = "ru">
    /// Увеличивает количество продуктов на один.
    /// </summary>
    public void IncQuantity()
    {
        if (Quantity == Product.Quantity)
            return;

        Quantity++;
    }

    /// <summary xml:lang = "ru">
    /// Уменьшает количество продуктов на один до 1.
    /// </summary>
    public void DecQuantity()
    {
        if (Quantity == 1)
            return;

        Quantity--;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Product.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OrderItem item && Equals(item);

    /// <inheritdoc/>
    public bool Equals(OrderItem? other) => Product.Equals(other?.Product);
}
