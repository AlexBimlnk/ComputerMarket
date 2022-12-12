using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class CatalogFilterTests
{
    [Fact(DisplayName = $"The {nameof(CatalogFilter)} can be created withput parameters.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutParameters()
    {
        // Arrange
        CatalogFilter filter = null!;
        
        // Act
        var exception = Record.Exception(() => filter = new CatalogFilter());

        // Assert
        exception.Should().BeNull();
        filter.SearchString.Should().BeNull();
        filter.SelectedTypeId.Should().BeNull();
        filter.PropertiesWithValues.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact(DisplayName = $"The {nameof(CatalogFilter)} can be created with parameters.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithParameters()
    {
        // Arrange
        CatalogFilter filter = null!;
        var searchString = "string";
        var idOfType = 1;
        var values = new HashSet<(ID, string)>();

        // Act
        var exception = Record.Exception(() => filter = new CatalogFilter(searchString, idOfType, values));

        // Assert
        exception.Should().BeNull();
        filter.SearchString.Should().Be(searchString);
        filter.SelectedTypeId.Should().Be(idOfType);
        filter.PropertiesWithValues.Should().BeEquivalentTo(values);
    }
}
