using Import.Logic.Abstractions;
using Import.Logic.Models;

using Microsoft.Extensions.Logging;

using Moq;

namespace Import.Logic.Tests;
public class APIProductFetcherTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIProductFetcher<FakeModel>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIProductFetcher<FakeModel>(logger, deserializer, converter));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can't create without logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Arrange
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIProductFetcher<FakeModel>(null!, deserializer, converter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without deserializer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutDeserializer()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIProductFetcher<FakeModel>>>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIProductFetcher<FakeModel>(logger, null!, converter));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can't create without converter.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConverter()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIProductFetcher<FakeModel>>>(MockBehavior.Strict);
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);

        // Act
        var exception = Record.Exception(() =>
            _ = new APIProductFetcher<FakeModel>(logger, deserializer, null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The instance can fetch products.")]
    [Trait("Category", "Unit")]
    public void CanFetchProducts()
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIProductFetcher<FakeModel>>>();
        var deserializer = new Mock<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var converter = new Mock<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        var externalModels = new[]
        {
            new FakeModel(),
            new FakeModel(),
        };

        deserializer.Setup(x => x.Deserialize(It.IsAny<string>()))
            .Returns(externalModels);

        converter.Setup(x => x.Convert(It.IsAny<FakeModel>()))
            .Returns(new Product(new(1, Provider.Ivanov), new(1), 1));

        var fetcher = new APIProductFetcher<FakeModel>(
            logger,
            deserializer.Object,
            converter.Object);

        var expectedResult = new[]
        {
            new Product(new(1, Provider.Ivanov), new(1), 1),
            new Product(new(1, Provider.Ivanov), new(1), 1)
        };

        // Act
        var result = fetcher.FetchProducts("some request");

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
    public void CanNotFetchProductsIfGivenBadRequest(string request)
    {
        // Arrange
        var logger = Mock.Of<ILogger<APIProductFetcher<FakeModel>>>();
        var deserializer = Mock.Of<IDeserializer<string, FakeModel[]>>(MockBehavior.Strict);
        var converter = Mock.Of<IConverter<FakeModel, Product>>(MockBehavior.Strict);

        var fetcher = new APIProductFetcher<FakeModel>(
            logger,
            deserializer,
            converter);

        // Act
        var exception = Record.Exception(() =>
            _ = fetcher.FetchProducts(request));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    public record class FakeModel();
}
