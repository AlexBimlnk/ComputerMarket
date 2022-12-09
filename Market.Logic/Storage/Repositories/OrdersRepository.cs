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
using TUser = Models.User;

public sealed class OrdersRepository : IOrderRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<OrdersRepository> _logger;

    public OrdersRepository(IRepositoryContext context, ILogger<OrdersRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Order entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var order = ConvertToStorageModel(entity);
        order.User = default!;
        order.Items = order.Items.Select(x => { x.Product = default!; return x; }).ToList();

        token.ThrowIfCancellationRequested();

        await _context.Orders.AddAsync(order, token)
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

        var order = ConvertToStorageModel(entity);
        order.User = default!;
        order.Items = order.Items.Select(x => { x.Product = default!; return x; }).ToList();

        _context.Orders.Remove(order);
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
            User = ConvertToStorageModel(order.Creator),
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
        Product = ConvertToStorageModel(item.Product)
    };

    private static Order ConvertFromStorageModel(TOrder order)
    {
        var newOrder = new Order(
            new ID(order.Id),
            new User(
                new ID(order.UserId),
                new AuthenticationData(
                    order.User.Email,
                    new Password(order.User.Password),
                    order.User.Login),
                (UserType)order.User.UserTypeId),
            order.Date,
            order.Items.Select(x => ConvertFromStorageModel(x)).ToHashSet());

        newOrder.State = (OrderState)order.StateId;

        return newOrder;
    }

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
        Item = ConvertToStorageModel(product.Item),
        ProviderId = product.Provider.Key.Value,
        Provider = ConvertToStorageModel(product.Provider)
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

    private static TUser ConvertToStorageModel(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.AuthenticationData.Login,
        Email = user.AuthenticationData.Email,
        Password = user.AuthenticationData.Password.Value,
        UserTypeId = (short)user.Type
    };

    private User? ConvertFromStorageModel(TUser user)
    {
        if (!Enum.IsDefined(typeof(UserType), (int)user.UserTypeId))
        {
            _logger.LogWarning(
                "The user with user type id: {UserTypeId} can't be converted",
                user.UserTypeId);
            return null;
        }

        return new User(
            id: new ID(user.Id),
            new AuthenticationData(
                user.Email,
                new Password(user.Password),
                user.Login),
            (UserType)user.UserTypeId);
    }

    #endregion
}
