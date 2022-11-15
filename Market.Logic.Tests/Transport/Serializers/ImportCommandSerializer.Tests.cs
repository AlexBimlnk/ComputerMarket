using Market.Logic.Commands.Import;
using Market.Logic.Models;
using Market.Logic.Transport.Serializers;

namespace Market.Logic.Tests.Transport.Serializers;
public class ImportCommandSerializerTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new ImportCommandSerializer());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can serialize.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(SerializeData))]
    public void CanSerialize(ImportCommand command, string expectedResult)
    {
        // Arrange
        var serializer = new ImportCommandSerializer();

        string actual = null!;

        // Act
        var exception = Record.Exception(() =>
            actual = serializer.Serialize(command));

        // Assert
        exception.Should().BeNull();
        actual.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = $"The instance can't serialize null.")]
    [Trait("Category", "Unit")]
    public void CanNotSerializeNull()
    {
        // Arrange
        var serializer = new ImportCommandSerializer();

        // Act
        var exception = Record.Exception(() =>
            _ = serializer.Serialize(null!));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    public readonly static TheoryData<ImportCommand, string> SerializeData = new()
    {
        {
            new SetLinkCommand(
                new("some id"),
                internalItemId: new(1),
                externalItemId: new(2),
                new Provider(
                    id: new InternalID(1),
                    "Some Provider Name",
                    new Margin(2),
                    new PaymentTransactionsInformation(
                        inn: "0123456789",
                        bankAccount: "01234012340123401234"))),
            
            /*lang=json,strict*/@"{""internal_id"":1,""external_id"":2,""provider"":""some_provider_name"",""type"":""set_link"",""id"":""some id""}"
        },
        {
            new DeleteLinkCommand(
                new("some id"),
                externalItemId: new(2),
                new Provider(
                    id: new InternalID(1),
                    "Some Provider Name",
                    new Margin(2),
                    new PaymentTransactionsInformation(
                        inn: "0123456789",
                        bankAccount: "01234012340123401234"))),
            
            /*lang=json,strict*/@"{""external_id"":2,""provider_id"":1,""type"":""delete_link"",""id"":""some id""}"
        }
    };
}
