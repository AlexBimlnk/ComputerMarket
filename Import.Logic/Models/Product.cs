﻿using General.Models;

namespace Import.Logic.Models;

/// <summary xml:lang = "ru">
/// Продукт.
/// </summary>
public sealed class Product : IMappableEntity<InternalID, ExternalID>
{
    private InternalID? _internalID;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="Product"/>
    /// </summary>
    /// <param name="externalID" xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </param>
    /// <param name="price" xml:lang = "ru">
    /// Цена.
    /// </param>
    /// <param name="quantity" xml:lang = "ru">
    /// Количество.
    /// </param>
    /// <param name="metadata" xml:lang = "ru">
    /// Метаданные о продукте, полученные от поставщика.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Когда <paramref name="quantity"/> меньше нуля.
    /// </exception>
    public Product(
        ExternalID externalID,
        Price price,
        int quantity,
        string? metadata = null)
    {
        ExternalID = externalID;
        Price = price;

        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity = quantity;
        Metadata = metadata;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    public ExternalID ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </summary>
    public InternalID InternalID =>
        _internalID ?? throw new InvalidOperationException("Product isn't mapped.");

    /// <summary xml:lang = "ru">
    /// Цена.
    /// </summary>
    public Price Price { get; }

    /// <summary xml:lang = "ru">
    /// Количество.
    /// </summary>
    public int Quantity { get; }

    /// <summary xml:lang = "ru">
    /// Указывает на наличие связи с внутренним продуктом.
    /// </summary>
    public bool IsMapped { get; private set; }

    /// <summary xml:lang="ru">
    /// Метаданные о продукте, полученные от поставщика.
    /// </summary>
    public string? Metadata { get; }

    /// <summary xml:lang = "ru">
    /// Устанавливает связь с внутренним продуктом.
    /// </summary>
    /// <param name="internalID" xml:lang = "ru">
    /// Идентификатор внутреннего продукта.
    /// </param>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если продукт был уже связан.
    /// </exception>
    public void MapTo(InternalID internalID)
    {
        if (IsMapped)
            throw new InvalidOperationException("Product already is mapped.");

        _internalID = internalID;
        IsMapped = true;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        $"{{ {ExternalID}, {(IsMapped ? $"{InternalID}, " : string.Empty)}" +
        $"Price: {Price.Value}, Quantity: {Quantity} }}";
}