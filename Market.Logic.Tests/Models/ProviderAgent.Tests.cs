﻿using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class ProviderAgentTests
{
    [Fact(DisplayName = $"The {nameof(ProviderAgent)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        ProviderAgent providerAgent = null!;
        var provider = TestHelper.GetOrdinaryProvider();
        var user = new User(
            new ID(1),
            TestHelper.GetOrdinaryAuthenticationData(),
            UserType.Agent);

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
    public void CanNotCreateWhenUserIsNotAgent()
    {
        //Arrange
        var user = new User(
            new ID(1),
            TestHelper.GetOrdinaryAuthenticationData(),
            UserType.Customer);

        // Act
        var exception = Record.Exception(() => _ = new ProviderAgent(
            agent: user,
            TestHelper.GetOrdinaryProvider()));

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
            TestHelper.GetOrdinaryProvider()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(ProviderAgent)} cannot create witout {nameof(Provider)}.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutProvider()
    {
        // Act
        var exception = Record.Exception(() => _ = new ProviderAgent(
            agent: new User(
                new ID(1),
                TestHelper.GetOrdinaryAuthenticationData(),
                UserType.Agent),
            provider: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}