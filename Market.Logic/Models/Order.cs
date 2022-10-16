using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Заказ.
/// </summary>
public sealed class Order
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Order"/>.
    /// </summary>
    /// <param name="user" xml:lang = "ru">Пользотваль создавший заказ.</param>
    /// <param name="products" xml:lang = "ru">Продукты для добавления в заказ.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="user"/> или <paramref name="products"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru"
    /// >Если в заказе имеются одинаковые продукты или продуктов нет.
    /// </exception>
    public Order(User user, Dictionary<Product, int> products)
    {
        Creator = user ?? throw new ArgumentNullException(nameof(user));
        State = OrderState.PaymentWait;
        OrderDate = DateTime.Now;

        ArgumentNullException.ThrowIfNull(products, nameof(products));

        if (products.DistinctBy(x => x.Key).Count() != products.Count)
            throw new InvalidOperationException("Order can't contain similar positions");

        Items = products
            .Where(x => x.Value > 0)
            .Select(x => new OrderItem(x.Key, x.Value));

        if (!Items.Any())
            throw new InvalidOperationException("Order can't contains zero items");
    }

    /// <summary xml:lang = "ru">
    /// Позиции в заказе.
    /// </summary>
    public IEnumerable<OrderItem> Items { get; }

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
    public DateTime OrderDate { get; set; }

    /// <summary xml:lang = "ru">
    /// Метод высчитывающий итоговую стоимость заказа.
    /// </summary>
    /// <returns xml:lang = "ru">Итоговая стоимость.</returns>
    public decimal GetSumCost() => Items.Sum(x => x.Product.FinalCost * x.Quantity);
}