using Market.Logic.Models;

using ItemDescription = Market.Logic.Storage.Models.ItemDescription;
using TItem = Market.Logic.Storage.Models.Item;
using TItemProperty = Market.Logic.Storage.Models.ItemProperty;
using TItemType = Market.Logic.Storage.Models.ItemType;
using TProduct = Market.Logic.Storage.Models.Product;
using TPropertyGroup = Market.Logic.Storage.Models.PropertyGroup;
using TProvider = Market.Logic.Storage.Models.Provider;
using TUser = Market.Logic.Storage.Models.User;

namespace Market.Logic.Tests;

public static class TestHelper
{
    #region User

    public static AuthenticationData GetOrdinaryAuthenticationData(string? email = null, string? login = "login1", string? pass = null) => new(
            email ?? "mAiL33@mail.ru",
            new Password(pass ?? "12345678"),
            login);

    public static User GetOrdinaryUser(long id = 1, AuthenticationData data = null!, UserType type = UserType.Customer) => new(
            new ID(id),
            data ?? GetOrdinaryAuthenticationData(),
            type);

    public static TUser GetStorageUser(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.AuthenticationData.Login,
        Email = user.AuthenticationData.Email,
        Password = user.AuthenticationData.Password.Value,
        UserTypeId = (short)user.Type,
    };

    #endregion

    #region Provider

    public static PaymentTransactionsInformation GetOrdinaryPaymentTransactionsInformation(string? inn = null, string? acc = null) => new(
            inn ?? "1234567890",
            acc ?? "01234012340123401234");

    public static Provider GetOrdinaryProvider(long id = 1, string? name = null, PaymentTransactionsInformation? info = null) => new(
            new ID(id),
            name ?? "Provider Name",
            margin: new Margin(1.3m),
            info ?? GetOrdinaryPaymentTransactionsInformation());

    public static TProvider GetStorageProvider(Provider provider) => new()
    {
        Id = provider.Key.Value,
        Name = provider.Name,
        Margin = provider.Margin.Value,
        Inn = provider.PaymentTransactionsInformation.INN,
        BankAccount = provider.PaymentTransactionsInformation.BankAccount
    };

    public static ProviderAgent GetOrdinaryProviderAgent() => new(
            agent: new User(
                new ID(1),
                GetOrdinaryAuthenticationData(),
                UserType.Agent),
            GetOrdinaryProvider());

    #endregion

    #region Item

    public static PropertyGroup GetOrdinaryPropertyGroup() => new(id: new ID(1), "Some property Group");

    public static ItemType GetOrdinaryItemType() => new(id: 1, "Som item type");

    public static ItemProperty GetOrdinaryItemProperty(
        long id = 1, 
        string name = "Some property name", 
        string? value = null,
        PropertyGroup? group = null,
        bool isFilterable = false,
        PropertyDataType type = PropertyDataType.String)
    {
        var property = new ItemProperty(
            new ID(id),
            name,
            group ?? GetOrdinaryPropertyGroup(),
            isFilterable,
            type);

        if (value is not null)
            property.Value = value;

        return property;
    }

    public static ItemDescription GetStorageItemPropertyWithOutItem(ItemProperty property)
    {
        var tProperty = new TItemProperty()
        {
            Id = property.Key.Value,
            Name = property.Name,
            GroupId = (int) property.Group.Id.Value,
            Group = new TPropertyGroup()
            {
                Id = (int) property.Group.Id.Value,
                Name = property.Group.Name
            },
            IsFilterable = property.IsFilterable,
            PropertyDataTypeId = property.ProperyDataType
        };

        return new ItemDescription()
        {
            PropertyId = tProperty.Id,
            Property = tProperty,
            PropertyValue = property.Value
        };
    }

    public static Item GetOrdinaryItem(
        long id = 1,
        ItemType? type = null,
        string name = "Some item name",
        IReadOnlyCollection<ItemProperty>? properties = null) => new(
            new ID(id),
            type ?? GetOrdinaryItemType(),
            name,
            properties ?? new ItemProperty[]
            {
                GetOrdinaryItemProperty(1, "PropName1", "Value1"),
                GetOrdinaryItemProperty(2, "PropName1", "Value2"),
                GetOrdinaryItemProperty(3, "PropName1", "Value3")
            });

    public static TItem GetStorageItem(Item item)
    {
        var tItem = new TItem()
        {
            Id = item.Key.Value,
            Name = item.Name,
            TypeId = item.Type.Id,
            Type = new TItemType()
            {
                Id = item.Type.Id,
                Name = item.Type.Name,
            }
        };

        tItem.Description = item.Properties
           .Select(x => GetStorageItemPropertyWithOutItem(x))
           .Select(x => { x.Item = tItem; x.ItemId = tItem.Id; return x; })
           .ToArray();

        return tItem;
    }

    #endregion

    #region Product

    public static Product GetOrdinaryProduct(Item? item = null, Provider? provider = null, decimal price = 100m, int quantity = 5) => new(
        item ?? GetOrdinaryItem(),
        provider ?? GetOrdinaryProvider(),
        new Price(price),
        quantity);

    public static TProduct GetStorageProduct(Product product) => new()
    {
        ProviderCost = product.ProviderCost,
        Quantity = product.Quantity,
        ItemId = product.Item.Key.Value,
        ProviderId = product.Provider.Key.Value,
        Provider = GetStorageProvider(product.Provider),
        Item = GetStorageItem(product.Item)
    };

    public static PurchasableEntity GetOrdinaryPurchasableEntity(Product? product = null, int quantity = 3) => 
        new(product ?? GetOrdinaryProduct(), quantity);

    #endregion

    public static Order WithState(this Order order, OrderState state)
    {
        order.State = state;
        return order;
    }
}
