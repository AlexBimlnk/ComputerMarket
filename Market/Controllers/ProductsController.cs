using Market.Logic;
using Market.Logic.Models;
using Market.Logic.Models.Abstractions;
using Market.Logic.Storage;
using Market.Logic.Storage.Repositories;
using Market.Models.Products;

using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang="ru">
/// Контроллер для поучения информации об товарах.
/// </summary>
public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ICatalog _catalog;
    
    /// <summary xml:lang="ru">
    /// Создаёт экземпляр класса <see cref="ProductsController"/>.
    /// </summary>
    /// <param name="catalog" xml:lang="ru">Каталог товаров.</param>
    /// <param name="logger" xml:lang="ru">Логгер.</param>
    /// <exception cref="ArgumentNullException" xml:lang="ru">Если один из параметров - <see langword="null"/>.</exception>
    public ProductsController(
        ICatalog catalog, 
        ILogger<ProductsController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <summary xml:lang="ru">
    /// Возвращает страницу со всеми категориями товаров.
    /// </summary>
    /// <returns xml:lang="ru"></returns>
    [HttpGet]
    public IActionResult Categories()
    {
        var types = _catalog.GetItemTypes();
        return View(types);
    }

    /// <summary xml:lang="ru">
    /// Возвращает страницу с каталогом товаров с заданными параметрами.
    /// </summary>
    /// <param name="searchString" xml:lang="ru">Посикаова строка.</param>
    /// <param name="typeId" xml:lang="ru">Индетификатор категории товара.</param>
    /// <returns xml:lang="ru"></returns>
    [HttpGet]
    public IActionResult Catalog(CatalogViewModel model)
    {
        model.Properties = GetProductsProperties(_catalog.GetProducts(
            model.TypeId is null
                ? new CatalogFilter(model.SearchString)
                : new CatalogFilter(typeId: model.TypeId)));

        var selectedValues = GetPropertiesValues(model.Params ?? "false");
        var filter = new CatalogFilter(model.SearchString, model.TypeId, selectedValues);
        model.Products = _catalog.GetProducts(filter);

        if (selectedValues.Any())
        {
            foreach (var value in filter.PropertiesWithValues)
            {
                if (model.Properties.ContainsKey(value.Item1))
                {
                    var newValue = new FilterValue(value.Item1, value.Item2);
                    newValue.Selected = true;
                    model.Properties[value.Item1].AddValue(newValue);
                }
            }
        } 

        return View(model);
    }

    /// <summary xml:lang="ru">
    /// Обновляет страницу с каталогом товаров.
    /// </summary>
    /// <param name="model" xml:lang="ru"></param>
    /// <returns xml:lang="ru"></returns>
    [HttpPost]
    public IActionResult Update(CatalogViewModel model)
    {
        var res = Request.Form["Selected"].ToString();

        model.Params = res;

        return RedirectToAction("Catalog", model);
    }

    /// <summary xml:lang="ru">
    /// Возвращает страницу продукта.
    /// </summary>
    /// <param name="itemId" xml:lang="ru"></param>
    /// <param name="providerId" xml:lang="ru"></param>
    /// <returns xml:lang="ru"></returns>
    [HttpGet]
    public IActionResult Product(long itemId, long providerId) 
    {

        var product = _catalog.GetProductByKey((new ID(providerId), new ID(itemId)));

        if (product is null)
        {
            Response.StatusCode = 404;
            return View(product);
        }

        return View(product);
    }

    private static IReadOnlyDictionary<ID, IFilterProperty> GetProductsProperties(IEnumerable<Product> products)
    {
        var result = new Dictionary<ID, IFilterProperty>();

        products
            .SelectMany(x => x.Item.Properties)
            .Where(x => x.Value is not null)
            .ToList()
            .ForEach(x =>
            {
                var property = new FilterProperty(x);
                var value = new FilterValue(property.Property.Key, x.Value!);

                if (!result.ContainsKey(x.Key))
                {
                    result.Add(x.Key, property);
                }

                result[x.Key].AddValue(value);
            });

        return result;
    }

    private static IReadOnlySet<(ID, string)> GetPropertiesValues(string request)
    {
        var result = new HashSet<(ID, string)>();

        foreach(var line in request.Split(',')) 
        { 
            if (!line.Contains("_"))
            {
                continue;
            }

            var value = line.Split('_');

            var isSucces = long.TryParse(value[1], out var propertyId);

            if (!isSucces)
            {
                continue;
            }

            result.Add((new ID(propertyId), value[0]));
        }

        return result;
    }
}
