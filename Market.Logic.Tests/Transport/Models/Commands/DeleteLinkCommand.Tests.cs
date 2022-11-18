using Market.Logic.Models;
using Market.Logic.Transport.Models.Commands.Import;

namespace Market.Logic.Tests.Transport.Models.Commands;

public class DeleteLinkCommandTests
{
    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can convert domain models.")]
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

        var domainCommand = new Logic.Commands.Import.DeleteLinkCommand(
            new("some id"),
            externalItemId: new(2),
            provider);

        var expectedResult = new DeleteLinkCommand("some id", Logic.Commands.CommandType.DeleteLink)
        {
            ExternalID = 2,
            ProviderID = 1
        };

        DeleteLinkCommand actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = DeleteLinkCommand.ToModel(domainCommand));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = $"The {nameof(DeleteLinkCommand)} can't convert null.")]
    [Trait("Category", "Unit")]
    public void CanNotConvertNull()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = DeleteLinkCommand.ToModel(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }
}
