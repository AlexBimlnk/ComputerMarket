using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class CatalogFilterTests
{
    [Fact(DisplayName = $"The {nameof(CatalogFilter)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        CatalogFilter filter = null!;
        
        // Act
        var exception = Record.Exception(() => filter = new CatalogFilter());

        // Assert
        exception.Should().BeNull();
        filter.SearchString.Should().BeNull();
        filter.SelectedType.Should().BeNull();
        filter.PropertiesWithValues.Should().NotBeNull().And.HaveCount(0);
    }
}
