using Import.Logic.Models;
using Import.Logic.Transport.Models;

namespace Import.Logic.Tests;
public class ProductsConverterTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ProductsConverter());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The instance can convert {nameof(ExternalProduct)}.")]
    [Trait("Category", "Unit")]
    public void CanConvertExternalProducts()
    {
        // Arrange
        var converter = new ProductsConverter();

        var externalProduct = new ExternalProduct
        {
            Id = 1,
            Name = "Some name",
            Price = 67,
            Quantity = 8,
            Description = Array.Empty<string>()
        };

        var expectedResult = new Product(
            externalID: new(1, Provider.Ivanov),
            price: new(67),
            quantity: 8);

        // Act
        var result = converter.Convert(externalProduct);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
            opt.Excluding(x => x.InternalID)
               .Excluding(x => x.Metadata));

        result.Metadata.Should().ContainAll(externalProduct.Description
            .Append(externalProduct.Name));
    }

    [Fact(DisplayName = $"The instance can convert {nameof(HornsAndHoovesProduct)}.")]
    [Trait("Category", "Unit")]
    public void CanConvertHornsAndHoovesProducts()
    {
        // Arrange
        var converter = new ProductsConverter();

        var externalProduct = new HornsAndHoovesProduct
        {
            Id = 1,
            Name = "Some name",
            Price = 76,
            Quantity = 8,
            Description = "some description",
            Type = "processor"
        };

        var expectedResult = new Product(
            externalID: new(1, Provider.HornsAndHooves),
            price: new(76),
            quantity: 8);

        // Act
        var result = converter.Convert(externalProduct);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
            opt.Excluding(x => x.InternalID)
               .Excluding(x => x.Metadata));

        result.Metadata.Should().ContainAll(
            externalProduct.Name, 
            externalProduct.Description, 
            externalProduct.Type);
    }

    [Fact(DisplayName = $"The instance can't convert null.")]
    [Trait("Category", "Unit")]
    public void CanNotConvertNull()
    {
        // Arrange
        var converter = new ProductsConverter();

        // Act
        var exception = Record.Exception(() =>
            _ = converter.Convert((ExternalProduct)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
