using Market.Logic.Models;

namespace Market.Logic.Reports;

/// <summary>
/// Отчет о прдожах.
/// </summary>
public sealed class Report
{
    /// <summary>
    /// Создает новый объекти типа <see cref="Report"/>.
    /// </summary>
    /// <param name="provider"> Поставщик по которому составляется отчет.  </param>
    /// <param name="soldProductsCount"> Кол-во проданных продуктов. </param>
    /// <param name="productMVP"> Самый продоваемый продукт. </param>
    /// <param name="totalProfit"> Общая прибыль. </param>
    /// <exception cref="ArgumentNullException">
    /// Если любой из входных аргументов оказался <see langword="null"/>.
    /// </exception>
    public Report(
        Provider provider,
        long soldProductsCount,
        Product productMVP,
        decimal totalProfit)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        ProductMVP = productMVP ?? throw new ArgumentNullException(nameof(productMVP));
        SoldProductsCount = soldProductsCount;
        TotalProfit = totalProfit;
    }

    /// <summary>
    /// Поставщик по которому составляется отчет.
    /// </summary>
    public Provider Provider { get; }

    /// <summary>
    /// Кол-во проданных продуктов.
    /// </summary>
    public long SoldProductsCount { get; }

    /// <summary>
    /// Самый продоваемый продукт.
    /// </summary>
    public Product ProductMVP { get; }

    /// <summary>
    /// Общая прибыль.
    /// </summary>
    public decimal TotalProfit { get; }
}
