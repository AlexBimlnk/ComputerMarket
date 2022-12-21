using General.Storage;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;

namespace Market.Logic.Reports;

/// <summary>
/// Создатель отчетов.
/// </summary>
public sealed class ReportBuilder : IReportBuilder
{
    private BuilderConfig _config = new();
    private readonly IOrderRepository _orderRepository;
    private readonly IProvidersRepository _providerRepository;

    /// <summary>
    /// Создает новый объект типа <see cref="ReportBuilder"/>.
    /// </summary>
    /// <param name="orderRepository"> Репозиторий заказаов. </param>
    /// <param name="providerRepository"> Репозиторий провайдеров. </param>
    /// <exception cref="ArgumentNullException">
    /// Если любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public ReportBuilder(
        IOrderRepository orderRepository,
        IProvidersRepository providerRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
    }

    private IReadOnlyDictionary<Product, long> CreateProductMap(Provider provider)
    {
        var possibleOrders = _orderRepository.GetEntities()
            .Where(x => x.State == OrderState.Received)
            .Where(x =>
                DateOnly.FromDateTime(x.OrderDate) >= _config.StartPeriod &&
                DateOnly.FromDateTime(x.OrderDate) <= _config.EndPeriod)
            .Where(x => x.Items.Any(item => item.Product.Provider.Equals(provider)));

        var soldProducts = possibleOrders
            .SelectMany(x => x.Items)
            .Where(x => x.Product.Provider.Equals(provider));

        var productsMap = new Dictionary<Product, long>();
        foreach (var i in soldProducts)
            if (productsMap.ContainsKey(i.Product))
                productsMap[i.Product] += i.Quantity;
            else
                productsMap.Add(i.Product, i.Quantity);

        return productsMap;
    }

    /// <inheritdoc/>
    public void SetProviderId(ID providerId) => _config.ProviderId = providerId;

    /// <inheritdoc/>
    public void SetStartPeriod(DateOnly date) => _config.StartPeriod = date;

    /// <inheritdoc/>
    public void SetEndPeriod(DateOnly date) => _config.EndPeriod = date;

    /// <inheritdoc/>
    public Report CreateReport()
    {
        if (_config.ProviderId is null || _config.StartPeriod is null || _config.EndPeriod is null)
            throw new InvalidOperationException("Can't create report without id or start/end date");

        var provider = _providerRepository.GetByKey(_config.ProviderId.Value)!;

        var productsMap = CreateProductMap(provider);

        var soldProdcutsCount = productsMap
            .Sum(x => x.Value);

        var productMVP = productsMap
            .MaxBy(x => x.Value)
            .Key;

        var totalProfit = productsMap
            .Select(x => x.Key.ProviderCost * x.Value)
            .Sum();

        return new Report(
            provider,
            soldProdcutsCount,
            productMVP,
            totalProfit);
    }

    private sealed class BuilderConfig
    {
        public ID? ProviderId { get; set; }
        public DateOnly? StartPeriod { get; set; }
        public DateOnly? EndPeriod { get; set; }
    }
}
