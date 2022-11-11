using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Market.Logic.Models;

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
        var password = new Password("password");
        
        // Act
        var exception = Record.Exception(() => data = new AuthenticationData(
            login,
            email,
            password));

        // Assert
        exception.Should().BeNull();
        data.Email.Should().Be(email);
        data.Login.Should().Be(login);
        data.Password.Should().Be(password);
    }

    [Fact(DisplayName = $"The {nameof(AuthenticationData)} cannot be created without password.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPassword()
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            login: "Login1",
            email: "mmail@mail.ru",
            password: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(AuthenticationData)} cannot be created with incorrect data type enum value.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithWrongAuthenticationDataTypeValue()
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            login: "login1",
            email: "mAiL33@mail.ru",
            password: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
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
    public void CanNotCreateWhenLoginIncorrect(string login)
    {
        // Act
        var exception = Record.Exception(() => _ = new AuthenticationData(
            login,
            email: "mAiL33@mail.ru",
            new Password("password")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Theory(DisplayName = $"The {nameof(AuthenticationData)} cannot be created when given email has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
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
            login: "login1",
            email,
            new Password("password")));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}
