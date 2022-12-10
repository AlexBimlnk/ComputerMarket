using Market.Logic.Models;
using Market.Logic.Models.Abstractions;

namespace Market.Logic.Tests.Models;

public class FilterPropertyTests
{
    [Fact(DisplayName = $"The {nameof(FilterProperty)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        FilterProperty property = null!;
        var itemProperty = TestHelper.GetOrdinaryItemProperty();

        // Act
        var exception = Record.Exception(() => property = new FilterProperty(itemProperty));

        // Assert
        exception.Should().BeNull();
        property.Property.Should().Be(itemProperty);
        property.Values.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact(DisplayName = $"The {nameof(FilterProperty)} can not be created when property is null.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithNullProperty()
    {
        // Act
        var exception = Record.Exception(() => _ = new FilterProperty(property: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(FilterProperty)} can add value.")]
    [Trait("Category", "Unit")]
    public void CanAddValue()
    {
        // Arrange
        var property = new FilterProperty(TestHelper.GetOrdinaryItemProperty());
        var value1 = TestHelper.GetOrdinaryFilterValue();
        var value2 = TestHelper.GetOrdinaryFilterValue();

        var expectedResult = new Dictionary<string, IFilterValue>()
        {
            {value1.Value, value1.WithCount(2) }
        };

        // Act
        property.AddValue(value1);
        property.AddValue(value2);

        // Assert
        property.Values.Should().HaveCount(1).And.BeEquivalentTo(expectedResult);
    }
}
