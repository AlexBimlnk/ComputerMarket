using Market.Logic.Models;

using TItem = Market.Logic.Storage.Models.Item;
using ItemDescription = Market.Logic.Storage.Models.ItemDescription;
using TItemProperty = Market.Logic.Storage.Models.ItemProperty;
using TPropertyGroup = Market.Logic.Storage.Models.PropertyGroup;
using TPropertyDataType = Market.Logic.Storage.Models.PropertyDataTypeId;
using TProduct = Market.Logic.Storage.Models.Product;
using TUser = Market.Logic.Storage.Models.User;
using TProvider = Market.Logic.Storage.Models.Provider;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

public abstract class RepositoryHelper
{
    #region Item
    protected static TItem ConvertToStorageModel(Item item)
    {
        var tItem = new TItem()
        {
            Id = item.Key.Value,
            Name = item.Name,
            TypeId = item.Type.Id,
        };

        tItem.Description = item.Properties
           .Select(x => ConvertToStorageModel(x))
           .Select(x => { x.Item = tItem; x.ItemId = tItem.Id; return x; })
           .ToArray();

        return tItem;
    }

    protected static Item ConvertFromStorageModel(TItem item) => new(
        new ID(item.Id),
        new ItemType(item.TypeId, item.Type.Name),
        item.Name,
        item.Description
            .Select(x => ConvertFromStorageModel(x))
            .ToArray());

    protected static ItemDescription ConvertToStorageModel(ItemProperty property)
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

    protected static ItemProperty ConvertFromStorageModel(ItemDescription property)
    {
        var group = property.Property.GroupId is not null
                ? new PropertyGroup(property.Property.GroupId.GetValueOrDefault(), property.Property.Group!.Name)
                : PropertyGroup.Default;

        var dProperty = new ItemProperty(
            new ID(property.PropertyId),
            property.Property.Name,
            group,
            property.Property.IsFilterable,
            (PropertyDataType)property.Property.PropertyDataTypeId);

        if (property.PropertyValue is not null)
        {
            dProperty.SetValue(property.PropertyValue);
        }

        return dProperty;
    }

    #endregion

    #region Product

    protected static TProduct ConvertToStorageModel(Product product) => new()
    {
        ProviderCost = product.ProviderCost,
        Quantity = product.Quantity,
        ItemId = product.Item.Key.Value,
        ProviderId = product.Provider.Key.Value,
    };

    protected static Product ConvertFromStorageModel(TProduct product)
      => new Product(
          ConvertFromStorageModel(product.Item),
          ConvertFromStorageModel(product.Provider),
          new Price(product.ProviderCost),
          product.Quantity);

    #endregion

    #region User
    protected static TUser ConvertToStorageModel(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.AuthenticationData.Login,
        Email = user.AuthenticationData.Email,
        Password = user.AuthenticationData.Password.Value,
        UserTypeId = (short)user.Type
    };

    protected static User? ConvertFromStorageModel(TUser user, ILogger logger)
    {
        if (!Enum.IsDefined(typeof(UserType), (int)user.UserTypeId))
        {
            logger.LogWarning(
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

    #region Provider

    protected static TProvider ConvertToStorageModel(Provider provider) => new()
    {
        Id = provider.Key.Value,
        Name = provider.Name,
        Margin = provider.Margin.Value,
        Inn = provider.PaymentTransactionsInformation.INN,
        BankAccount = provider.PaymentTransactionsInformation.BankAccount
    };

    protected static Provider ConvertFromStorageModel(TProvider provider) =>
        new Provider(
            new ID(provider.Id),
            provider.Name,
            new Margin(provider.Margin),
            new PaymentTransactionsInformation(provider.Inn, provider.BankAccount));
    
    #endregion
}
