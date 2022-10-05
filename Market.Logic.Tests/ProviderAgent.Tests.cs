using Market.Logic.Models;

namespace Market.Logic.Tests;

public class ProviderAgentTests
{
    [Fact(DisplayName = $"The {nameof(ProviderAgent)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        ProviderAgent providerAgent = null!;
        var provider = new Provider("Company Name", 1.1m,
            new PaymentTransactionsInformation("1234567890", "1234"));
        var user = new User("login", new Password("12345"), UserType.Agent);

        // Act
        var exception = Record.Exception(() => providerAgent = new ProviderAgent(
            user,
            provider));

        // Assert
        exception.Should().BeNull();
        providerAgent.Agent.Should().Be(user);
        providerAgent.Provider.Should().Be(provider);
    }

    [Fact(DisplayName = $"The {nameof(ProviderAgent)} cannot be created when user type is not agent.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWhenProviderAgentMarginLessOne()
    {
        //Arrange
        var provider = new Provider("Company Name", 1.1m,
            new PaymentTransactionsInformation("1234567890", "1234"));
        var user = new User("login", new Password("12345"), UserType.Customer);

        // Act
        var exception = Record.Exception(() => _ = new ProviderAgent(
            agent: user,
            provider: provider));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(ProviderAgent)} cannot create witout {nameof(User)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutUser()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProviderAgent(
            agent: null!,
            provider: new Provider("Company Name", 1.1m,
            new PaymentTransactionsInformation("1234567890", "1234"))));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProviderAgent)} cannot create witout {nameof(Provider)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProvider()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProviderAgent(
            agent: new User("login", new Password("12345"), UserType.Agent),
            provider: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}