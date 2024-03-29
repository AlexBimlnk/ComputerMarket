﻿namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Сущность приобретаемая пользователем.
/// </summary>
public sealed class PurchasableEntity : IEquatable<PurchasableEntity>
{
    private bool? _isApproved = null;

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="PurchasableEntity"/>.
    /// </summary>
    /// <param name="product"  xml:lang = "ru">Товар в позиции заказа.</param>
    /// <param name="quantity"  xml:lang = "ru">Количество товар в позиции.</param>
    /// <exception cref="ArgumentNullException"  xml:lang = "ru">
    /// Если <paramref name="product"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException"  xml:lang = "ru">
    /// Если <paramref name="quantity"/> <= 0 или больше количества продуктов.
    /// </exception>
    public PurchasableEntity(Product product, int quantity)
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

    public bool? IsApproved 
    {
        get => _isApproved;
        set
        {
            if (_isApproved is not null)
                throw new InvalidOperationException("Can't change entity aprove state after set");
            _isApproved = value;
        }
    }

    /// <summary xml:lang = "ru">
    /// Увеличивает количество продуктов на один до доступного количества продуктов.
    /// </summary>
    public void IncQuantity()
    {
        // Todo: состояние продукта может быть не актуальным.
        // Нужно наверное ссылку на реп хранить, чтобы актуальный продукт получать.

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
    public override int GetHashCode() => HashCode.Combine(Product, Quantity);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is PurchasableEntity entity && Equals(entity);

    /// <inheritdoc/>
    public bool Equals(PurchasableEntity? other) => Product.Equals(other?.Product);
}
