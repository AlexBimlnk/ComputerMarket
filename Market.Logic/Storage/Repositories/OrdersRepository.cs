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

/// <summary xml:lang="ru">
/// Репозиторий заказаов.
/// </summary>
public sealed class OrdersRepository : IOrderRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<OrdersRepository> _logger;

    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="Order"/>.
    /// </summary>
    /// <param name="context" xml:lang="ru">Контекст базы данных.</param>
    /// <param name="logger" xml:lang="ru">Логгер.</param>
    /// <exception cref="ArgumentNullException" xml:lang="ru">Если один из параметров - <see langword="null"/>.</exception>
    public OrdersRepository(IRepositoryContext context, ILogger<OrdersRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Order entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();
        
        var order = ConvertToStorageModel(entity);
        order.User = _context.Users.Single(x => x.Id == order.UserId);
        order.Items = order.Items.Select(x => { x.Product = _context.Products.Single(xx => xx.ItemId == x.ItemId && xx.ProviderId == x.ProviderId); return x; }).ToList();


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

        var deleteOrder = _context.Orders.SingleOrDefault(x => x.Id == order.Id);

        if (deleteOrder is null)
            return;

        _context.Orders.Remove(deleteOrder);
    }
    
    /// <inheritdoc/>
    public Order? GetByKey(ID key) =>
        _context.Orders
        .AsNoTrackingWithIdentityResolution()
        .Include(x => x.User)
        .Include(x => x.Items)
        .ThenInclude(x => x.Product)
        .ThenInclude(x => x.Provider)
        .Include(x => x.Items)
        .ThenInclude(x => x.Product)
        .ThenInclude(x => x.Item)
        .ThenInclude(x => x.Type)
        .Include(x => x.Items)
        .ThenInclude(x => x.Product)
        .ThenInclude(x => x.Item)
        .ThenInclude(x => x.Description)
        .ThenInclude(x => x.Property)
        .ThenInclude(x => x.Group)
        .Where(x => x.Id == key.Value)
        .Select(x => ConvertFromStorageModel(x))
        .SingleOrDefault();

    /// <inheritdoc/>
    public IEnumerable<Order> GetEntities() =>
         _context.Orders
            .ToList()
            .Select(x => ConvertFromStorageModel(x));

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public void UpdateState(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var storageOrder = ConvertToStorageModel(order);

        var newOrder = _context.Orders.SingleOrDefault(x => x.Id == order.Key.Value);

        if (newOrder is null)
            return;

        newOrder.StateId = storageOrder.StateId;

        _context.Orders.Update(newOrder);
    }

    /// <inheritdoc/>
    public IEnumerable<Order> GetProviderOrders(Provider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        return _context.Orders
            .ToList()
            .Where(x => x.Items.Any(x => x.ProviderId == provider.Key.Value))
            .Select(x => ConvertFromStorageModel(x));
    }

    /// <inheritdoc/>
    public void ProviderArpove(Order order, Provider provider, bool value)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(provider);

        var approveItems = _context.OrdersItems
            .Where(x => 
                order.Key.Value == x.OrderId && 
                provider.Key.Value == x.ProviderId)
            .ToList();

        foreach(var item in approveItems)
        {
            item.ApprovedByProvider = value;

            _context.OrdersItems.Update(item);
        }
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
        Product = ConvertToStorageModel(item.Product),
        ApprovedByProvider = item.IsApproved
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
            item.Quantity)
        { 
            IsApproved = item.ApprovedByProvider
        };

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
        new ItemType(item.TypeId, item.Type.Name, item.Type.Url),
        item.Name,
        item.Description
            .Select(x => ConvertFromStorageModel(x))
            .ToArray(), 
        item.Url);

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
        BankAccount = provider.PaymentTransactionsInformation.BankAccount,
        IsAproved = provider.IsAproved
    };

    private static Provider ConvertFromStorageModel(TProvider provider) =>
       new Provider(
           new ID(provider.Id),
           provider.Name,
           new Margin(provider.Margin),
           new PaymentTransactionsInformation(provider.Inn, provider.BankAccount))
       {
           IsAproved = provider.IsAproved
       };

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
