using Market.Logic.Storage.Models;

using Market.Logic.Storage.Models;

using DItemProperty = Market.Logic.Models.ItemProperty;
using DPropertyGroup = Market.Logic.Models.PropertyGroup;
using DItem = Market.Logic.Models.Item;
using DItemType = Market.Logic.Models.ItemType;
using DPropertyDataType = Market.Logic.Models.PropertyDataType;

using TProduct = Market.Logic.Storage.Models.Product;
using TUser = Market.Logic.Storage.Models.User;


namespace Market.Logic.Storage.Repositories;

public class RepositoryHelper
{
    private static Item ConvertToStorageModel(DItem dItem)
    {
        var item = new Item()
        {
            Id = dItem.Key.Value,
            Name = dItem.Name,
            TypeId = dItem.Type.Id,
        };

        item.Description = dItem.Properties
           .Select(x => ConvertToStorage(x))
           .Select(x => { x.Item = item; x.ItemId = item.Id; return x; })
           .ToArray();

        return item;
    }

    private static DItem ConvertFromStorage(Item item) => new DItem(
        new ID(item.Id),
        new DItemType(item.TypeId, item.Type.Name),
        item.Name,
        item.Description
            .Select(x => ConvertFromStorage(x))
            .ToArray());

    public static ItemDescription ConvertToStorage(DItemProperty dProperty)
    {
        var property = new ItemProperty()
        {
            Id = dProperty.Key.Value,
            Name = dProperty.Name,
            GroupId = dProperty.Group.Id,
            Group = new PropertyGroup()
            {
                Id = dProperty.Group.Id,
                Name = dProperty.Group.Name
            },
            IsFilterable = dProperty.IsFilterable,
            PropertyDataTypeId = (PropertyDataTypeId)dProperty.ProperyDataType
        };

        return new ItemDescription()
        {
            PropertyId = property.Id,
            Property = property,
            PropertyValue = dProperty.Value
        };
    }

    private static DItemProperty ConvertFromStorage(ItemDescription property)
    {
        var group = property.Property.GroupId is not null
                ? new DPropertyGroup(property.Property.GroupId.GetValueOrDefault(), property.Property.Group!.Name)
                : DPropertyGroup.Default;

        var dProperty = new DItemProperty(
            new ID(property.PropertyId),
            property.Property.Name,
            group,
            property.Property.IsFilterable,
            (DPropertyDataType)property.Property.PropertyDataTypeId);

        if (property.PropertyValue is not null)
        {
            dProperty.SetValue(property.PropertyValue);
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

    private static Product ConvertFromStorage(TProduct product)
      => new Product(
          new Item(
              new ID(product.ItemId),
              new ItemType(product.Item.TypeId, product.Item.Type.Name),
              product.Item.Name,
              product.Item.Description
                .Select(x => new ItemProperty(
                    x.Property.Name,
                    x.PropertyValue ?? string.Empty))
                .ToArray()),
          new Provider(
              new ID(product.ProviderId),
              product.Provider.Name,
              new Margin(product.Provider.Margin),
              new PaymentTransactionsInformation(
                  product.Provider.Inn,
                  product.Provider.BankAccount)),
          new Price(product.ProviderCost),
          product.Quantity);

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
}
