using General.Transport;

using Market.Logic.Transport.Configuration;

namespace Market.Logic.Tests.Transport;
public class SenderConfigutationValidatorTests
{
    [Fact(DisplayName = $"The instance can create.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new SenderConfigurationValidator<FakeConfiguration>());

        // Assert
        exception.Should().BeNull();
    }

    [Theory(DisplayName = $"The instance can validate configuration.")]
    [Trait("Category", "Unit")]
    [MemberData(nameof(ValidateData))]
    public void CanValidateConfiguration(string destiantion, bool expectedResult)
    {
        // Arrange
        var configuration = new FakeConfiguration(destiantion);
        var validator = new SenderConfigurationValidator<FakeConfiguration>();

        // Act
        var actual = validator.Validate(nameof(FakeConfiguration), configuration);

        // Assert
        actual.Succeeded.Should().Be(expectedResult);
    }

    [Fact(DisplayName = $"The instance can validate null configuration.")]
    [Trait("Category", "Unit")]
    public void CanValidateNullConfiguration()
    {
        // Arrange
        var validator = new SenderConfigurationValidator<FakeConfiguration>();

        // Act
        var actual = validator.Validate(nameof(FakeConfiguration), null!);

        // Assert
        actual.Failed.Should().BeTrue();
    }

    public static readonly TheoryData<string, bool> ValidateData = new()
    {
        {
            null!,
            false
        },
        {
            "",
            false
        },
        {
            "  ",
            false
        },
        {
            " \n\r ",
            false
        },
        {
            "https://localhost:44376/api/courses",
            true
        },
    };

    public record FakeConfiguration(string Destination) : ITransportSenderConfiguration;
}
