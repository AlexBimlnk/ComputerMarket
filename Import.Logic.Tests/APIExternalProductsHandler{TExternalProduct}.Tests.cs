using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Import.Logic.Abstractions;
using Import.Logic.Models;
using Import.Logic.Transport.Receivers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Import.Logic.Tests;
public class APIExternalProductsHandlerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var fetcher = Mock.Of<IAPIProductFetcher<FakeExternalProduct>>(MockBehavior.Strict);
        var mapper = Mock.Of<IMapper<Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => 
            _ = new APIExternalProductsHandler<FakeExternalProduct>(
                logger,
                fetcher,
                mapper));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);
        var historyConverter = Mock.Of<IConverter<FakeModel, History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIExternalProductsHandler<FakeExternalProduct>(
            null!,
            deserializer,
            historyRecorder,
            converter,
            historyConverter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);
        var historyConverter = Mock.Of<IConverter<FakeModel, History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            null!,
            historyRecorder,
            converter,
            historyConverter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutHistoryReader()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);
        var historyConverter = Mock.Of<IConverter<FakeModel, History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            deserializer,
            null!,
            converter,
            historyConverter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without history reader.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConverter()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);
        var historyConverter = Mock.Of<IConverter<FakeModel, History>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            deserializer,
            historyRecorder,
            null!,
            historyConverter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without history converter.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutHistoryConverter()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() => _ = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            deserializer,
            historyRecorder,
            converter,
            null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can fetch products.")]
    [Trait("Category", "Unit")]
    public async Task CanFetchProductsAsync()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>();
        var deserializer = new Mock<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var historyRecorder = new Mock<IHistoryRecorder>(MockBehavior.Strict);
        var converter = new Mock<IConverter<FakeModel, Product>>(MockBehavior.Strict);
        var historyConverter = new Mock<IConverter<FakeModel, History>>(MockBehavior.Strict);

        var externalModels = new[]
        {
            new FakeModel(),
            new FakeModel(),
        };

        deserializer.Setup(x => x.Deserialize(It.IsAny<string>()))
            .Returns(externalModels);

        converter.Setup(x => x.Convert(It.IsAny<FakeModel>()))
            .Returns(new Product(new(1, Provider.Ivanov), new(1), 1));

        historyConverter.Setup(x => x.Convert(It.IsAny<FakeModel>()))
            .Returns(It.IsAny<History>());

        historyRecorder.Setup(x => x.RecordHistoryAsync(
                It.IsAny<IReadOnlyCollection<History>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var fetcher = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            deserializer.Object,
            historyRecorder.Object,
            converter.Object,
            historyConverter.Object);

        var expectedResult = new[]
        {
            new Product(new(1, Provider.Ivanov), new(1), 1),
            new Product(new(1, Provider.Ivanov), new(1), 1)
        };

        // Act
        var result = await fetcher.FetchProductsAsync("some request");

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
            opt.Excluding(x => x.InternalID)
            .RespectingRuntimeTypes());
    }

    [Theory(DisplayName = $"The instance can't fetch products if given bad request.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("  ")]
    [InlineData("\t \n\r ")]
    [InlineData("")]
    public async Task CanNotFetchProductsIfGivenBadRequestAsync(string request)
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIExternalProductsHandler<FakeExternalProduct>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var historyRecorder = Mock.Of<IHistoryRecorder>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);
        var historyConverter = Mock.Of<IConverter<FakeModel, History>>(MockBehavior.Strict);

        // Act
        var fetcher = new APIExternalProductsHandler<FakeExternalProduct>(
            logger,
            deserializer,
            historyRecorder,
            converter,
            historyConverter);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            _ = await fetcher.FetchProductsAsync(request));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public record class FakeExternalProduct();
}
