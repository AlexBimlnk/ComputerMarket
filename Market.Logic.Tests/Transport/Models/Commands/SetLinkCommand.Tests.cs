using Market.Logic.Models;
using Market.Logic.Transport.Models.Commands.Import;

namespace Market.Logic.Tests.Transport.Models.Commands;
public class SetLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can convert domain models.")]
    [Trait("Category", "Unit")]
    public void CanConvertDomainModel()
    {
        // Arrange
        var provider = new Provider(
            id: new ID(1),
            "Some Provider Name",
            new Margin(2),
            new PaymentTransactionsInformation(
                inn: "0123456789",
                bankAccount: "01234012340123401234"));

        var domainCommand = new Logic.Commands.Import.SetLinkCommand(
            new("some id"),
            internalItemId: new(1),
            externalItemId: new(2),
            provider);

        var expectedResult = new SetLinkCommand("some id", Logic.Commands.CommandType.SetLink)
        {
            InternalID = 1,
            ExternalID = 2,
            ProviderID = 1
        };

        SetLinkCommand actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = SetLinkCommand.ToModel(domainCommand));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(SetLinkCommand)} can't convert null.")]
    [Trait("Category", "Unit")]
    public void CanNotConvertNull()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = SetLinkCommand.ToModel(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
