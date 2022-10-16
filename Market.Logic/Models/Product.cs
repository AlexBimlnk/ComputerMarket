using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Продукт.
/// </summary>
public sealed class Product
{
    private readonly Price _price;

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
        _price = price;
        Provider = provider ?? throw new ArgumentNullException(nameof(item));
        
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        
        Quantity = quantity;
    }

    /// <summary xml:lang = "ru">
    /// Описание продукта.
    /// </summary>
    public Item Item { get; }

    /// <summary xml:lang = "ru">
    /// Цена назначаенная поставщиком.
    /// </summary>
    public decimal ProviderCost => _price.Value;

    /// <summary xml:lang = "ru">
    /// Итоговая цена продукта.
    /// </summary>
    public decimal FinalCost => _price.Value * Provider.Margin;

    /// <summary xml:lang = "ru">
    /// Поставщик продукта.
    /// </summary>
    public Provider Provider { get; }

    /// <summary xml:lang = "ru">
    /// Колличестов продукта.
    /// </summary>
    public int Quantity { get; private set; }
}
