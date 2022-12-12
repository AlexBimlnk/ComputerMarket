using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;

namespace Market.Models.Products;

public class CatalogViewModel
{
    public string? SearchString { get; set; }

    public int? TypeId { get; set; }

    public IEnumerable<Product> Products { get; set; }


    public IReadOnlyDictionary<ID, IFilterProperty> Properties { get; set; }
}
