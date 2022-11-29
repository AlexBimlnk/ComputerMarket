using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;
public interface IItemsRepository : IKeyableRepository<Item, ID>
{
    public void AddOrUpdate(Item item);
}
