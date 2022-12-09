using General.Storage;

using Market.Logic.Models;
using Market.Logic.Reports;

using Moq;

namespace Market.Logic.Tests.Models;

public class ReportBuilderTests
{
    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        ReportBuilder reportBuilder = null!;
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => reportBuilder = new ReportBuilder(
            orderRepository,
            providerRepository));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can set provider id.")]
    [Trait("Category", "Unit")]
    public void CanSetProviderID()
    {
        // Arrange
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(orderRepository, providerRepository);

        // Act
        var exception = Record.Exception(() => reportBuilder.SetProviderId(new ID(2)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can set start period.")]
    [Trait("Category", "Unit")]
    public void CanSetStartPeriod()
    {
        // Arrange
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(orderRepository, providerRepository);

        // Act
        var exception = Record.Exception(() => 
            reportBuilder.SetStartPeriod(DateOnly.FromDateTime(DateTime.Now)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can set end period.")]
    [Trait("Category", "Unit")]
    public void CanSetEndPeriod()
    {
        // Arrange
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(orderRepository, providerRepository);

        // Act
        var exception = Record.Exception(() =>
            reportBuilder.SetEndPeriod(DateOnly.FromDateTime(DateTime.Now)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can set end period.")]
    [Trait("Category", "Unit")]
    public void CanCreateReport()
    {
        // Arrange
        var provider = TestHelper.GetOrdinaryProvider();
        var providerId = provider.Key;

        var providerRepository = new Mock<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);
        providerRepository.Setup(x => x.GetByKey(providerId))
            .Returns(provider);

        var orders = new List<Order>()
        {
            new Order(
                new ID(1),
                TestHelper.GetOrdinaryUser(),
                DateTime.Now,
                new HashSet<PurchasableEntity>()
                {
                    new PurchasableEntity(
                        TestHelper.GetOrdinaryProduct(
                            TestHelper.GetOrdinaryItem(1),
                            provider,
                            100),
                        2)
                }).WithState(OrderState.ProviderAnswerWait),
            new Order(
                new ID(1),
                TestHelper.GetOrdinaryUser(),
                DateTime.Now,
                new HashSet<PurchasableEntity>()
                {
                    new PurchasableEntity(
                        TestHelper.GetOrdinaryProduct(
                            TestHelper.GetOrdinaryItem(1),
                            provider,
                            100),
                        2),
                    new PurchasableEntity(
                        TestHelper.GetOrdinaryProduct(
                            TestHelper.GetOrdinaryItem(2),
                            TestHelper.GetOrdinaryProvider(9999),
                            120),
                        2)
                }).WithState(OrderState.Ready),
            new Order(
                new ID(1),
                TestHelper.GetOrdinaryUser(),
                DateTime.Now,
                new HashSet<PurchasableEntity>()
                {
                    new PurchasableEntity(
                        TestHelper.GetOrdinaryProduct(
                            TestHelper.GetOrdinaryItem(2),
                            provider,
                            120),
                        5)
                }).WithState(OrderState.Ready),
        };

        var orderRepository = new Mock<IKeyableRepository<Order, ID>>(MockBehavior.Strict);
        orderRepository.Setup(x => x.GetEntities())
            .Returns(orders);

        var expectedReport = new Report(
            provider,
            soldProductsCount: 7,
            productMVP: TestHelper.GetOrdinaryProduct(
                TestHelper.GetOrdinaryItem(2),
                provider,
                120),
            totalProfit: 2 * 100 + 5 * 120);

        var reportBuilder = new ReportBuilder(
            orderRepository.Object,
            providerRepository.Object);

        reportBuilder.SetProviderId(providerId);
        reportBuilder.SetStartPeriod(DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(1)));
        reportBuilder.SetEndPeriod(DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)));

        // Act
        var actual = reportBuilder.CreateReport();

        // Assert
        actual.Should().BeEquivalentTo(expectedReport);
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can without provider id.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProviderId()
    {
        // Arrange
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(
            orderRepository,
            providerRepository);

        reportBuilder.SetStartPeriod(DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(1)));
        reportBuilder.SetEndPeriod(DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)));

        // Act
        var exception = Record.Exception(() => _ = reportBuilder.CreateReport());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can without start period.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutStartPeriod()
    {
        // Arrange
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(
            orderRepository,
            providerRepository);

        reportBuilder.SetProviderId(new ID(1));
        reportBuilder.SetEndPeriod(DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)));

        // Act
        var exception = Record.Exception(() => _ = reportBuilder.CreateReport());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(ReportBuilder)} can without end period.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutEndPeriod()
    {
        // Arrange
        var providerRepository = Mock.Of<IKeyableRepository<Provider, ID>>(MockBehavior.Strict);
        var orderRepository = Mock.Of<IKeyableRepository<Order, ID>>(MockBehavior.Strict);

        var reportBuilder = new ReportBuilder(
            orderRepository,
            providerRepository);

        reportBuilder.SetProviderId(new ID(1));
        reportBuilder.SetStartPeriod(DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)));

        // Act
        var exception = Record.Exception(() => _ = reportBuilder.CreateReport());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}
