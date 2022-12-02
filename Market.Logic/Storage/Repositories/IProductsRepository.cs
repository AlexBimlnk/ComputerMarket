using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;
public interface IProductsRepository : IKeyableRepository<Item, ID>
{
    public void Update(Item item);

    public Task AddOrUpdateProductAsync(Product product, CancellationToken token = default);

    public Task AddProductRangeAsync(IReadOnlyCollection<Product> products);

    public void RemoveProduct(Product product);

    public void RemoveProduct((ID, ID) key);

    public Task<bool> ContainsProductAsync((ID, ID) key, CancellationToken token = default);

    public Product? GetProduct((ID, ID) key);

    public IEnumerable<Product> GetAllProducts();

    public IEnumerable<Product> GetAllItemProducts(Item item);

    public IEnumerable<Product> GetAllProviderProducts(Provider provider);
}
