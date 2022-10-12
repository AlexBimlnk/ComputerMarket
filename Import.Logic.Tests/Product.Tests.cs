using Import.Logic.Models;

namespace Import.Logic.Tests;

public class ProductTests
{
    [Fact(DisplayName = $"The {nameof(Product)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        Product product = null!;
        var externalID = new ExternalID(1, Provider.HornsAndHooves);
        var price = new Price(100m);
        var quantity = 5;
        var metadata = "some metadata";

        // Act
        var exception = Record.Exception(() => product = new Product(
            externalID,
            price,
            quantity,
            metadata));

        // Assert
        exception.Should().BeNull();
        product.ExternalID.Should().Be(externalID);
        product.Price.Should().Be(price);
        product.Quantity.Should().Be(quantity);
        product.IsMapped.Should().BeFalse();
        product.Metadata.Should().Be(metadata);
    }

    [Fact(DisplayName = $"The {nameof(Product)} can be created without metadata.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutMetadata()
    {
        // Arrange
        Product product = null!;
        var externalID = new ExternalID(1, Provider.HornsAndHooves);
        var price = new Price(100m);
        var quantity = 5;

        // Act
        var exception = Record.Exception(() => product = new Product(
            externalID,
            price,
            quantity,
            metadata: null!));

        // Assert
        exception.Should().BeNull();
        product.ExternalID.Should().Be(externalID);
        product.Price.Should().Be(price);
        product.Quantity.Should().Be(quantity);
        product.IsMapped.Should().BeFalse();
        product.Metadata.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot be created when quantity less 0.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenProductLessZero()
    {
        // Act
        var exception = Record.Exception(() => _ = new Product(
            externalID: new(1, Provider.HornsAndHooves),
            price: new(100m),
            quantity: -5));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} cannot get internal id when is not mapped.")]
    [Trait("Category", "Unit")]
    public void CanNotGetInternalIDWhenIsNotMapped()
    {
        // Arrange
        var product = new Product(
            externalID: new(1, Provider.HornsAndHooves),
            price: new(100m),
            quantity: 5);

        // Act
        var exception = Record.Exception(() => _ = product.InternalID);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(Product)} can be mapped.")]
    [Trait("Category", "Unit")]
    public void CanBeMapped()
    {
        // Arrange
        var product = new Product(
            externalID: new(1, Provider.HornsAndHooves),
            price: new(100m),
            quantity: 5);

        var internalID = new InternalID(1);

        // Act
        product.MapTo(internalID);

        // Assert
        product.InternalID.Should().Be(internalID);
        product.IsMapped.Should().Be(true);
    }

    [Fact(DisplayName = $"The {nameof(Product)} can't be mapped when already mapped.")]
    [Trait("Category", "Unit")]
    public void CanNotBeMappedWhenAlreadyMapped()
    {
        // Arrange
        var product = new Product(
            externalID: new(1, Provider.HornsAndHooves),
            price: new(100m),
            quantity: 5);

        var internalIDFirst = new InternalID(1);
        var internalIDSecond = new InternalID(2);

        product.MapTo(internalIDFirst);

        // Act
        var exception = Record.Exception(() => product.MapTo(internalIDSecond));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}