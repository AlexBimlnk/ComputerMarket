﻿using Market.Logic.Models;

namespace Market.Logic.Tests;

public class UserTests
{
    [Fact(DisplayName = $"The {nameof(User)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        User user = null!;
        var login = "login";
        var password = new Password("password");
        var type = UserType.Customer;
        var email = "mail.ru";

        // Act
        var exception = Record.Exception(() => user = new User(
            login,
            password,
            email,
            type));

        // Assert
        exception.Should().BeNull();
        user.Login.Should().Be(login);
        user.Password.Should().Be(password);
        user.Type.Should().Be(type);
        user.Email.Should().Be(email);
    }

    [Fact(DisplayName = $"The {nameof(User)} cannot be created without password.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPassword()
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            login: "login",
            password: null!,
            email: "mail.ru",
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(User)} cannot be created with incorrect user type enum value.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithWronUserTypeValue()
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            login: "login",
            password: new Password("12345"),
            email: "mail.ru",
            type: (UserType)1234));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(User)} cannot be created when given login has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    public void CanNotCreateWhenLoginIncorrect(string login)
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            login: login,
            password: new Password("12345"),
            email: "mail.ru",
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(User)} cannot be created when given email has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    public void CanNotCreateWhenEmailIncorrect(string email)
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            login: "login",
            password: new Password("12345"),
            email,
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}