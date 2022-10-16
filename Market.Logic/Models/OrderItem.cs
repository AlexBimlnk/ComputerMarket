namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Позиция в заказе.
/// </summary>
public sealed class OrderItem
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="OrderItem"/>.
    /// </summary>
    /// <param name="product"  xml:lang = "ru">Товар в позиции заказа.</param>
    /// <param name="quantity"  xml:lang = "ru">Колличесиво товар в позиции.</param>
    /// <exception cref="ArgumentNullException"  xml:lang = "ru">Если <paramref name="product"/> - <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"  xml:lang = "ru"> Если <paramref name="quantity"/> <= 0.</exception>
    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        Price = product.FinalCost;

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = quantity;
    }

    /// <summary  xml:lang = "ru">
    ///  Продукт.
    /// </summary>
    public Product Product { get; }

    /// <summary  xml:lang = "ru">
    /// Цена продукта.
    /// </summary>
    public decimal Price { get; }

    /// <summary  xml:lang = "ru">
    /// Колличество продуктов.
    /// </summary>
    public int Quantity { get; }

}
