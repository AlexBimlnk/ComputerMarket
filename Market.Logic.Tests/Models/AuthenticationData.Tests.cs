﻿using Market.Logic.Models;

namespace Market.Logic.Tests.Models;

public class AuthenticationDataTests
{
    [Fact(DisplayName = $"The {nameof(AuthenticationData)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        AuthenticationData data = null!;
        var login = "login1";
        var email = "mAiL33@mail.ru";
        var password = new Password("12345678");

        // Act
        var exception = Record.Exception(() => data = new AuthenticationData(
            email,
            password,
            login));

        // Assert
        exception.Should().BeNull();
        data.Email.Should().Be(email);
        data.Password.Should().Be(password);
        data.Login.Should().Be(login);
    }

    [Fact(DisplayName = $"The {nameof(AuthenticationData)} can be created without login.")]
    [Trait("Category", "Unit")]
    public void CanBeCreatedWithoutLogin()
    {
        // Arrange
        AuthenticationData data = null!;
        var email = "mAiL33@mail.ru";
        var password = new Password("12345678");

        // Act
        var exception = Record.Exception(() => data = new AuthenticationData(
            email,
            password));

        // Assert
        exception.Should().BeNull();
        data.Email.Should().Be(email);
        data.Password.Should().Be(password);
    }

    [Fact(DisplayName = $"The {nameof(AuthenticationData)} can not be created without password.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutPassword()
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            email: "mAiL33@mail.ru",
            password: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = $"The {nameof(AuthenticationData)} cannot be created when given login has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("ab123")]
    [InlineData("abcde123456abcde123456abcde123456abcde123456123")]
    [InlineData("abcd1234^$@#0)")]
    [InlineData("{}//^6786996")]
    public void CanNotCreateWhenLoginIncorrect(string login)
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            email: "mAiL33@mail.ru",
            new Password("12345678"),
            login));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(AuthenticationData)} cannot be created when given email has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("@r")]
    [InlineData("1234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678123456781234567812345678@mail.ru")]
    [InlineData("mailmail.ru")]
    [InlineData("mail@mailru")]
    [InlineData("@mail.ru")]
    [InlineData("mail@mail.ru.ru")]
    [InlineData("mail@mail.")]
    public void CanNotCreateWhenEmailIncorrect(string email)
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            email,
            new Password("12345678"),
            login: "login1"));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(AuthenticationData)} cannot be created when given login has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    [InlineData("ab123")]
    [InlineData("abcde123456abcde123456abcde123456abcde123456123")]
    [InlineData("abcd1234^$@#0)")]
    [InlineData("{}//^6786996")]
    public void CanNotSetIncorrectLogin(string login)
    {
        // Arrange
        var data = new AuthenticationData(
            email: "mAiL33@mail.ru",
            new Password("12345678"));

        // Act
        var exception = Record.Exception(() => data.Login = login);

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = $"The {nameof(AuthenticationData)} can get not setted login.")]
    [Trait("Category", "Unit")]
    public void CanNotGetLoginWhenHeIsNotSet()
    {
        // Arrange
        var email = "mAiL33@mail.ru";
        var data = new AuthenticationData(
            email,
            new Password("12345678"));

        // Act
        var exception = Record.Exception(() => _ = data.Login);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}
