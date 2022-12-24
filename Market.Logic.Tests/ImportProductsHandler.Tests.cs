using General.Storage;
using General.Transport;

using Market.Logic.Models;
using Market.Logic.Storage.Repositories;
using Market.Logic.Transport.Models;

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
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository,
            productsRepository,
            logger));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository,
            productsRepository,
            logger: null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer: null!,
            itemRepository,
            providerRepository,
            productsRepository,
            logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutItemRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer,
            itemRepository: null!,
            providerRepository,
            productsRepository,
            logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProviderRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository: null!,
            productsRepository,
            logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ImportProductsHandler)} can't create without repository.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProductRepository()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository,
            productRepository: null!,
            logger));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(ImportProductsHandler)} can handle command.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(DeserializeParameters))]
    public async Task CanHandleRequestAsync(
        string request, 
        IReadOnlyCollection<UpdateByProduct> addedProducts)
    {
        // Arrange
        var logger = Mock.Of<ILogger<ImportProductsHandler>>();
        var deserializer = new Mock<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = new Mock<IProductsRepository>(MockBehavior.Strict);

        var product = TestHelper.GetOrdinaryProduct();

        deserializer.Setup(x => x.Deserialize(request))
            .Returns(addedProducts);

        var repositoryGetCallBack = 0;
        productsRepository.Setup(x => x.GetByKey(
                It.IsAny<(ID, ID)>()))
            .Returns(product)
            .Callback(() => repositoryGetCallBack++);

        var repositoryAddCallBack = 0;
        productsRepository.Setup(x => x.AddOrUpdateAsync(
                product,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => repositoryAddCallBack++);

        var repositorySaveCallback = 0;
        productsRepository.Setup(x => x.Save())
            .Callback(() => repositorySaveCallback++);

        var handler = new ImportProductsHandler(
            deserializer.Object,
            itemRepository,
            providerRepository,
            productsRepository.Object,
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
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);

        var handler = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository,
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
        var deserializer = Mock.Of<IDeserializer<string, IReadOnlyCollection<UpdateByProduct>>>(MockBehavior.Strict);
        var itemRepository = Mock.Of<IItemsRepository>(MockBehavior.Strict);
        var providerRepository = Mock.Of<IProvidersRepository>(MockBehavior.Strict);
        var productsRepository = Mock.Of<IProductsRepository>(MockBehavior.Strict);

        var request = "some request";
        using var cts = new CancellationTokenSource();

        var handler = new ImportProductsHandler(
            deserializer,
            itemRepository,
            providerRepository,
            productsRepository,
            logger);

        // Act
        cts.Cancel();
        var exception = await Record.ExceptionAsync(async () =>
            await handler.HandleAsync(request, cts.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>();
    }

    public readonly static TheoryData<string, IReadOnlyCollection<UpdateByProduct>> DeserializeParameters = new()
    {
        {
            /*lang=json,strict*/
            @"[
                {""external_id"":1,""internal_id"":1,""provider_id"":1,""price"":100.0,""quantity"":5},
                {""external_id"":4,""internal_id"":2,""provider_id"":2,""price"":55.0,""quantity"":3}
            ]",
            new []
            {
                new UpdateByProduct(
                    externalID: new(1),
                    internalID: new(1),
                    providerID: new(1),
                    new Price(100.0m),
                    quantity: 5
                ),
                new UpdateByProduct(
                    externalID: new(4),
                    internalID: new(2),
                    providerID: new(2),
                    new Price(55.0m),
                    quantity: 3
                )
            }
        },
        {
            /*lang=json,strict*/@"[]",
            Array.Empty<UpdateByProduct>()
        }
    };
}
