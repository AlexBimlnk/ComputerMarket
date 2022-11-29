using Market.Logic.Models;

using TItem = Market.Logic.Storage.Models.Item;
using TItemType = Market.Logic.Storage.Models.ItemType;
using ItemDescription = Market.Logic.Storage.Models.ItemDescription;
using TItemProperty = Market.Logic.Storage.Models.ItemProperty;
using TPropertyGroup = Market.Logic.Storage.Models.PropertyGroup;
using TPropertyDataType = Market.Logic.Storage.Models.PropertyDataTypeId;
using TProduct = Market.Logic.Storage.Models.Product;
using TUser = Market.Logic.Storage.Models.User;
using TUserType = Market.Logic.Storage.Models.UserType;
using TProvider = Market.Logic.Storage.Models.Provider;
using Xunit.Sdk;

namespace Market.Logic.Tests;

public static class TestHelper
{
    public static AuthenticationData GetOrdinaryAuthenticationData(string? email = null, string? login = null) => new(
            email ?? "mAiL33@mail.ru",
            new Password("12345678"),
            login ?? "login1");

    public static User GetOrdinaryUser(long id = 1, AuthenticationData data = null!, UserType type = UserType.Customer) => new(
            new ID(id),
            data ?? GetOrdinaryAuthenticationData(),
            type);

    public static IReadOnlyCollection<User> GetUsersCollection() => new[]
    {
        GetOrdinaryUser(1, GetOrdinaryAuthenticationData("mail1@mail.ru", "login1"), UserType.Customer),
        GetOrdinaryUser(2, GetOrdinaryAuthenticationData("mail2@mail.ru", "login2"), UserType.Agent),
    };

    public static TUser GetStorageUser(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.AuthenticationData.Login,
        Email= user.AuthenticationData.Email,
        Password = user.AuthenticationData.Password.Value,
        UserTypeId = (short)user.Type,
    };

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

    public static PropertyGroup GetOrdinaryPropertyGroup() => new(id: 1, "Some property Group");

    public static ItemType GetOrdinaryItemType() => new(id: 1, "Som item type");

    public static ItemProperty GetOrdinaryItemProperty() => new(
            new ID(1), 
            name: "Some property name", 
            GetOrdinaryPropertyGroup(), 
            isFilterable: false, 
            PropertyDataType.String);

    public static ItemDescription GetStorageItemProperty(ItemProperty property)
    {
        var tProperty = new TItemProperty()
        {
            Id = property.Key.Value,
            Name = property.Name,
            GroupId = property.Group.Id,
            Group = new TPropertyGroup()
            {
                Id = property.Group.Id,
                Name = property.Group.Name
            },
            IsFilterable = property.IsFilterable,
            PropertyDataTypeId = (TPropertyDataType)property.ProperyDataType
        };

        return new ItemDescription()
        {
            PropertyId = tProperty.Id,
            Property = tProperty,
            PropertyValue = property.Value
        };
    }

    public static IReadOnlyCollection<ItemProperty> GetOrdinaryItemProperties() => new[]
    {
        new ItemProperty(new ID(1), "Property1", new  PropertyGroup(1, "Group 1"), false, PropertyDataType.String),
    };

    public static Item GetOrdinaryItem(
        long id = 1, 
        ItemType? type = null, 
        string name = "Some item name", 
        IReadOnlyCollection<ItemProperty>? properties = null) => new(
            new ID(id),
            type ?? GetOrdinaryItemType(),
            name,
            properties ?? GetOrdinaryItemProperties());

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
           .Select(x => GetStorageItemProperty(x))
           .Select(x => { x.Item = tItem; x.ItemId = tItem.Id; return x; })
           .ToArray();

        return tItem;
    }

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

    public static IReadOnlySet<PurchasableEntity> GetOrdinaryPurchasableEntities() => new HashSet<PurchasableEntity>()
    {
        new PurchasableEntity(GetOrdinaryProduct(GetOrdinaryItem(1, name: "Item1"), price: 20m, quantity: 5),1),
        new PurchasableEntity(GetOrdinaryProduct(GetOrdinaryItem(2, name: "Item2"), price: 60m, quantity: 10),3)
    };
}
