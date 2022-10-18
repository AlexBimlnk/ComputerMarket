namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Корзина в которую будут добавляться продукты.
/// </summary>
public sealed class Basket
{
    private readonly HashSet<PurchasableEntity> _items = new();

    /// <summary xml:lang = "ru">
    /// Добавляет продукт в корзину.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Добавляемый продукт.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="product"/> - <see langword="null"/>.
    /// </exception>
    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var item = new PurchasableEntity(product, 1);
        if (!_items.Add(item))
        {
            _items.TryGetValue(item, out var sameItem);
            sameItem!.IncQuantity();
        }
    }

    /// <summary xml:lang = "ru">
    /// Удаляет элемент корзины соотвествующего продукта.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Продукт, по которому удаляется элемент из корзины.</param>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если элемента с продуктом <paramref name="product"/> нет в корзине.
    /// </exception>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="product"/> - <see langword="null"/>.
    /// </exception>
    public void Remove(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var entity = new PurchasableEntity(product, 1);
        if (!_items.Remove(entity))
            throw new InvalidOperationException("Can't remove product which is not in basket");
    }

    /// <summary xml:lang = "ru">
    /// Получает элемент корзины соответвующего продукта.
    /// </summary>
    /// <param name="product" xml:lang = "ru">Проудкт, по которуму ищется элемент в корзине.</param>
    /// <returns>Найденый элемент из корзины с продуктом - <paramref name="product"/>.</returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="product"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException" xml:lang = "ru">
    /// Если <paramref name="product"/> отсутсвует в корзине.
    /// </exception>
    public PurchasableEntity GetBasketItem(Product product)
    {
        var item = new PurchasableEntity(product ?? throw new ArgumentNullException(nameof(product)), 1);
        _items.TryGetValue(item, out var resItem);
        return resItem ?? throw new InvalidOperationException("Can't get product which is not in basket");
    }

    /// <summary xml:lang = "ru">
    /// Коллекция элементов, находящихся в корзине.
    /// </summary>
    public IReadOnlyCollection<PurchasableEntity> Items => _items;
}
