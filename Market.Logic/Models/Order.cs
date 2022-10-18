namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Заказ.
/// </summary>
public sealed class Order
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Order"/>.
    /// </summary>
    /// <param name="user" xml:lang = "ru">Пользователь создавший заказ.</param>
    /// <param name="items" xml:lang = "ru">Продукты для добавления в заказ.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="user"/> или <paramref name="items"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если в заказе имеются одинаковые продукты или продуктов нет.
    /// </exception>
    public Order(User user, IReadOnlyCollection<PurchasableEntity> items)
    {
        Creator = user ?? throw new ArgumentNullException(nameof(user));
        State = OrderState.PaymentWait;
        OrderDate = DateTime.Now;

        ArgumentNullException.ThrowIfNull(items, nameof(items));

        Items = items
            .Where(x => x.Selected)
            .ToArray();

        if (!Items.Any())
            throw new InvalidOperationException("Order can't contains zero items");

        if (Items.GroupBy(x => x.Product).Count() != Items.Count)
            throw new InvalidOperationException("Order can't contains items with same product");
    }

    /// <summary xml:lang = "ru">
    /// Позиции в заказе.
    /// </summary>
    public IReadOnlyCollection<PurchasableEntity> Items { get; }

    /// <summary xml:lang = "ru">
    /// Пользователь создавший заказ.
    /// </summary>
    public User Creator { get; }

    /// <summary xml:lang = "ru">
    /// Состояние заказа.
    /// </summary>
    public OrderState State { get; private set; }

    /// <summary xml:lang = "ru">
    /// Дата создания заказа.
    /// </summary>
    public DateTime OrderDate { get; }

    /// <summary xml:lang = "ru">
    /// Метод высчитывающий итоговую стоимость заказа.
    /// </summary>
    /// <returns xml:lang = "ru">Итоговая стоимость.</returns>
    public decimal GetSumCost() => Items.Sum(x => x.Product.FinalCost * x.Quantity);
}