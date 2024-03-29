﻿using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class UserTests
{
    [Fact(DisplayName = $"The {nameof(User)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        User user = null!;
        var id = new ID(1);
        var data = TestHelper.GetOrdinaryAuthenticationData();
        var type = UserType.Customer;

        // Act
        var exception = Record.Exception(() => user = new User(
            id,
            data,
            type));

        // Assert
        exception.Should().BeNull();
        user.Key.Should().Be(id);
        user.AuthenticationData.Should().Be(data);
        user.Type.Should().Be(type);
    }

    [Fact(DisplayName = $"The {nameof(User)} cannot be created without authentication data.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPassword()
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            new ID(1),
            authenticationData: null!,
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(User)} cannot be created with incorrect user type enum value.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithWrongUserTypeValue()
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            new ID(1),
            TestHelper.GetOrdinaryAuthenticationData(),
            type: (UserType)1234));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}