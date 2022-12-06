using General.Models;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Заказ.
/// </summary>
public sealed class Order : IKeyable<ID>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Order"/>.
    /// </summary>
    /// <param name="user" xml:lang = "ru">Пользователь создавший заказ.</param>
    /// <param name="entities" xml:lang = "ru">Продукты для добавления в заказ.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="user"/> или <paramref name="entities"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если в заказе имеются одинаковые продукты или продуктов нет.
    /// </exception>
    public Order(User user, IReadOnlySet<PurchasableEntity> entities)
    {
        Creator = user ?? throw new ArgumentNullException(nameof(user));
        State = OrderState.PaymentWait;
        OrderDate = DateTime.Now;

        ArgumentNullException.ThrowIfNull(entities, nameof(entities));

        Items = entities
            .Where(x => x.Selected)
            .ToHashSet();

        if (!Items.Any())
            throw new InvalidOperationException("Order can't contains zero items");
    }

    /// <summary xml:lang = "ru">
    /// Позиции в заказе.
    /// </summary>
    public IReadOnlySet<PurchasableEntity> Items { get; }

    /// <summary xml:lang = "ru">
    /// Пользователь создавший заказ.
    /// </summary>
    public User Creator { get; }

    /// <summary xml:lang = "ru">
    /// Состояние заказа.
    /// </summary>
    public OrderState State { get; set; }

    /// <summary xml:lang = "ru">
    /// Дата создания заказа.
    /// </summary>
    public DateTime OrderDate { get; }

    /// <inheritdoc/>
    public ID Key => throw new NotImplementedException();

    /// <summary xml:lang = "ru">
    /// Метод высчитывающий итоговую стоимость заказа.
    /// </summary>
    /// <returns xml:lang = "ru">Итоговая стоимость.</returns>
    public decimal GetSumCost() => Items.Sum(x => x.Product.FinalCost * x.Quantity);
}