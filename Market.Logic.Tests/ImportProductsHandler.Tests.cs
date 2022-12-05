using General.Storage;
using General.Transport;

using Market.Logic.Models;

using Microsoft.Extensions.Logging;

using Moq;

namespace Market.Logic.Tests;
public class ImportProductsHandlerTests
{
    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var productsRepository = Mock.Of<IRepository<Product>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ImportProductsHandler(deserializer, productsRepository, logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IRepository<Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ImportProductsHandler(deserializer, productsRepository, logger: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IRepository<Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ImportProductsHandler(deserializer: null!, productsRepository, logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new ImportProductsHandler(deserializer, repositoryProduct: null!, logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(ImportProductsHandler)} can handle command.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public async Task CanHandleCommandAsync(string request, IReadOnlyCollection<Product> addedProducts)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var deserializer = new Mock<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);
        var repository = new Mock<IRepository<Product>>(MockBehavior.Strict);

        deserializer.Setup(x => x.Deserialize(request))
            .Returns(addedProducts);

        var repositoryAddCallBack = 0;
        repository.Setup(x => x.AddAsync(
                It.Is<Product>(x => addedProducts.Contains(x)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => repositoryAddCallBack++);

        var repositorySaveCallback = 0;
        repository.Setup(x => x.Save())
            .Callback(() => repositorySaveCallback++);

        var handler = new ImportProductsHandler(
            deserializer.Object,
            repository.Object,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () => await handler.HandleAsync(request));

        // Assert
        exception.Should().BeNull();
        repositoryAddCallBack.Should().Be(addedProducts.Count);
        repositorySaveCallback.Should().Be(1);
    }

    [Theory(DisplayName = $"The {nameof(ImportProductsHandler)} can't handle command if given bad request.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n\r ")]
    [InlineData("")]
    public async Task CanNotHandlerIfGivenBadRequestAsync(string request)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IRepository<Product>>(MockBehavior.Strict);

        var handler = new ImportProductsHandler(
            deserializer,
            productsRepository,
            logger);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can cancel operation.")]
    [Trait("Category", "Unit")]
    public async Task CanCancelOperationAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<Product>>>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IRepository<Product>>(MockBehavior.Strict);

        var request = "some request";
        using var cts = new CancellationTokenSource();

        var handler = new ImportProductsHandler(
            deserializer,
            productsRepository,
            logger);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    public readonly static TheoryData<string, IReadOnlyCollection<Product>> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"[
                {""external_id"":1,""internal_id"":1,""provider_id"":1,""price"":100.0,""quantity"":5},
                {""external_id"":4,""internal_id"":2,""provider_id"":2,""price"":55.0,""quantity"":3}
            ]",
            new []
            {
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(1), 
                    TestHelper.GetOrdinaryProvider(
                        2, 
                        info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
                            inn: "1111111111", 
                            acc: "11111111111111111111")),
                    price: 100m,
                quantity: 5),
                TestHelper.GetOrdinaryProduct(
                    TestHelper.GetOrdinaryItem(2),
                    TestHelper.GetOrdinaryProvider(
                        2,
                        info: TestHelper.GetOrdinaryPaymentTransactionsInformation(
                            inn: "1111111111",
                            acc: "11111111111111111111")),
                    price: 55m,
                quantity: 3)
            }
        },
        {
            /*lang=json,strict*/@"[]",
            Array.Empty<Product>()
        }
    };
}
