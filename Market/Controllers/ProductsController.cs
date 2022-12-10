using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Storage;
using Market.Logic.Storage.Repositories;
using Market.Models.Products;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace Market.Controllers;
public class ProductsController : Controller
{
    private readonly IItemsRepository _itemsRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductsController> _logger;
    private readonly Catalog _catalog;
    private readonly MarketContext _context;

    public ProductsController(IItemsRepository itemsRepository, IProductsRepository productsRepository, ILogger<ProductsController> logger, MarketContext context)
    {
        _itemsRepository = itemsRepository ?? throw new ArgumentNullException(nameof(itemsRepository));
        _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context;
        _catalog = new Catalog(productsRepository, itemsRepository);
    }

    [HttpGet]
    public IActionResult Categories()
    {
        var types = _catalog.GetItemTypes();
        return View(types);
    }

    [HttpGet]
    public IActionResult Catalog(string? searchString = null, int? typeId = null)
    {
        var filter = new CatalogFilter(searchString, typeId);

        var products = _catalog.GetProducts(filter);

        return View(new CatalogViewModel()
        {
            Products = products,
            SearchString = filter.SearchString,
            TypeId = filter.SelectedTypeId,
            Properties = GetProductsProperties(products)
        });
    }

    [HttpPost]
    public IActionResult Update(CatalogViewModel model)
    {
        var filter = new CatalogFilter(model.SearchString, model.TypeId);
        model.Products = _catalog.GetProducts(filter);

        return View("Catalog", model);
    }

    [HttpGet]
    public IActionResult Product(long itemId, long providerId) 
    {

        var product = _productsRepository.GetByKey((new ID(itemId), new ID(providerId)));

        if (product is null)
        {
            Response.StatusCode = 404;
            return View(product);
        }

        return View(product);
    }

    /// <summary xml:lang="ru">
    /// Возращает список свойств с их значениями выбранной колекции продктов.
    /// </summary>
    /// <param name="products" xml:lang="ru">Входная колекция продуктов.</param>
    /// <returns xml:lang="ru">Коллеккция свойств с их значениями из колекции продуктов.</returns>
    public static IReadOnlyDictionary<ID, IFilterProperty> GetProductsProperties(IEnumerable<Product> products)
    {
        var result = new Dictionary<ID, IFilterProperty>();

        products
            .SelectMany(x => x.Item.Properties)
            .Where(x => x.Value is not null)
            .ToList()
            .ForEach(x =>
            {
                var property = new FilterProperty(x);
                var value = new FilterValue(x.Value!);

                if (!result.ContainsKey(x.Key))
                {
                    result.Add(x.Key, property);
                }

                result[x.Key].AddValue(value);
            });

        return result;
    }
}
