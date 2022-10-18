namespace Market.Logic.Models;

public sealed class Basket
{
    private readonly HashSet<OrderItem> _items;

    public Basket()
    {
        _items = new();
    }

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var item = new OrderItem(product, 1);
        if (!_items.Add(item))
        {
            _items.TryGetValue(item, out var sameItem);
            sameItem!.IncQuantity();
        }
    }

    public void Remove(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (_items.RemoveWhere(item => item.Product.Equals(product)) == 0)
            throw new InvalidOperationException("Can't remove product which is not in basket");
    }

    public OrderItem GetBasketItem(Product product)
    {
        var item = new OrderItem(product ?? throw new ArgumentNullException(nameof(product)), 1);
        _items.TryGetValue(item, out var resItem);
        return resItem ?? throw new InvalidOperationException("Can't get product which is not in basket");
    }

    public IReadOnlyCollection<OrderItem> Items => _items;
}
