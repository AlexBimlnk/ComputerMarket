using Microsoft.Extensions.Logging;

using Market.Logic.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Logic.Storage.Repositories;

using TOrder = Storage.Models.Order;
using TOrderItem = Storage.Models.OrderItem;
using TItemDescription = Models.ItemDescription;
using TItem = Models.Item;
using TItemProperty = Models.ItemProperty;
using TProduct = Models.Product;
using TPropertyGroup = Models.PropertyGroup;
using TProvider = Models.Provider;

public sealed class OrderRepository : IOrderRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(IRepositoryContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task AddAsync(Order entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Orders.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }
    
    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Order entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Orders.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }
    
    /// <inheritdoc/>
    public void Delete(Order entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Orders.Remove(ConvertToStorageModel(entity));
    }
    
    /// <inheritdoc/>
    public Order? GetByKey(ID key) =>
        _context.Orders
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();

    /// <inheritdoc/>
    public IEnumerable<Order> GetEntities() =>
         _context.Orders
            .AsEnumerable()
            .Select(x => ConvertFromStorageModel(x))
            .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public void UpdateState(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _context.Orders.Update(ConvertToStorageModel(order));
    }

    #region Converters

    private static TOrder ConvertToStorageModel(Order order)
    {
        var tOrder = new TOrder()
        {
            Id = order.Key.Value,
            Date = order.OrderDate,
            StateId = (int)order.State,
            UserId = order.Creator.Key.Value,
            User = default!,
            Items = order.Items.Select(x => ConvertToStorageModel(x)).ToList()
        };

        tOrder.Items.Select(x =>
        {
            x.OrderId = tOrder.Id; return x;
        });

        return tOrder;
    }

    private static TOrderItem ConvertToStorageModel(PurchasableEntity item) => new()
    {
        ProviderId = item.Product.Provider.Key.Value,
        ItemId = item.Product.Item.Key.Value,
        Quantity = item.Quantity,
        PaidPrice = item.Product.FinalCost,
        Product = default!
    };

    private static Order ConvertFromStorageModel(TOrder order) =>
        new Order(
            new ID(order.Id),
            new User(
                new ID(order.UserId), 
                new AuthenticationData(
                    order.User.Email, 
                    new Password(order.User.Password), 
                    order.User.Login), 
                (UserType)order.User.UserTypeId),
            order.Items.Select(x => ConvertFromStorageModel(x)).ToHashSet());

    private static PurchasableEntity ConvertFromStorageModel(TOrderItem item) =>
        new PurchasableEntity(
            ConvertFromStorageModel(item.Product),
            item.Quantity);

    private static TItem ConvertToStorageModel(Item item)
    {
        var tItem = new TItem()
        {
            Id = item.Key.Value,
            Name = item.Name,
            TypeId = item.Type.Id,
            Type = new Models.ItemType()
            {
                Id = item.Type.Id,
                Name = item.Type.Name
            }
        };

        tItem.Description = item.Properties
           .Select(x => ConvertToStorageModel(x))
           .Select(x => { x.Item = tItem; x.ItemId = tItem.Id; return x; })
           .ToArray();

        return tItem;
    }

    private static Item ConvertFromStorageModel(TItem item) => new(
        new ID(item.Id),
        new ItemType(item.TypeId, item.Type.Name),
        item.Name,
        item.Description
            .Select(x => ConvertFromStorageModel(x))
            .ToArray());

    private static TItemDescription ConvertToStorageModel(ItemProperty property)
    {
        var tProperty = new TItemProperty()
        {
            Id = property.Key.Value,
            Name = property.Name,
            GroupId = (int)property.Group.Id.Value,
            Group = new TPropertyGroup()
            {
                Id = (int)property.Group.Id.Value,
                Name = property.Group.Name
            },
            IsFilterable = property.IsFilterable,
            PropertyDataTypeId = property.ProperyDataType
        };

        if (tProperty.GroupId == -1)
        {
            tProperty.GroupId = default;
            tProperty.Group = default;
        }

        return new TItemDescription()
        {
            PropertyId = tProperty.Id,
            Property = tProperty,
            PropertyValue = property.Value
        };
    }

    private static ItemProperty ConvertFromStorageModel(TItemDescription property)
    {
        var group = property.Property.GroupId is not null
                ? new PropertyGroup(new ID(property.Property.GroupId.GetValueOrDefault()), property.Property.Group!.Name)
                : PropertyGroup.Default;

        var dProperty = new ItemProperty(
            new ID(property.PropertyId),
            property.Property.Name,
            group,
            property.Property.IsFilterable,
            (PropertyDataType)property.Property.PropertyDataTypeId);

        if (property.PropertyValue is not null)
        {
            dProperty.Value = property.PropertyValue;
        }

        return dProperty;
    }

    private static TProduct ConvertToStorageModel(Product product) => new()
    {
        ProviderCost = product.ProviderCost,
        Quantity = product.Quantity,
        ItemId = product.Item.Key.Value,
        ProviderId = product.Provider.Key.Value,
    };

    private static Product ConvertFromStorageModel(TProduct product)
      => new Product(
          ConvertFromStorageModel(product.Item),
          ConvertFromStorageModel(product.Provider),
          new Price(product.ProviderCost),
          product.Quantity);

    private static TProvider ConvertToStorageModel(Provider provider) => new()
    {
        Id = provider.Key.Value,
        Name = provider.Name,
        Margin = provider.Margin.Value,
        Inn = provider.PaymentTransactionsInformation.INN,
        BankAccount = provider.PaymentTransactionsInformation.BankAccount
    };

    private static Provider ConvertFromStorageModel(TProvider provider) =>
       new Provider(
           new ID(provider.Id),
           provider.Name,
           new Margin(provider.Margin),
           new PaymentTransactionsInformation(provider.Inn, provider.BankAccount));

    #endregion
}
