using Market.Logic.Reports;

namespace Market.Logic.Tests.Models;

public class ReportTests
{
    [Fact(DisplayName = $"The {nameof(Report)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Report report = null!;
        var provider = TestHelper.GetOrdinaryProvider();
        var soldProductsCount = 12;
        var productMVP = TestHelper.GetOrdinaryProduct();
        var totalProfit = 123m;

        // Act
        var exception = Record.Exception(() => report = new Report(
            provider,
            soldProductsCount,
            productMVP,
            totalProfit));

        // Assert
        exception.Should().BeNull();
        report.Provider.Should().Be(provider);
        report.SoldProductsCount.Should().Be(soldProductsCount);
        report.ProductMVP.Should().Be(productMVP);
        report.TotalProfit.Should().Be(totalProfit);
    }

    [Fact(DisplayName = $"The {nameof(Report)} cannot be created without provider.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProvider()
    {
        // Act
        var exception = Record.Exception(() => _ = new Report(
            provider: null!,
            soldProductsCount: 13,
            productMVP: TestHelper.GetOrdinaryProduct(),
            totalProfit: 123.3m));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(Report)} cannot be created without product mvp.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProductMVP()
    {
        // Act
        var exception = Record.Exception(() => _ = new Report(
            TestHelper.GetOrdinaryProvider(),
            soldProductsCount: 13,
            productMVP: null!,
            totalProfit: 123.3m));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
