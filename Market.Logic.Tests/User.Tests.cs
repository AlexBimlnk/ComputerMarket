using Market.Logic.Models;

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

        // Act
        var exception = Record.Exception(() => user = new User(
            login,
            password,
            type));

        // Assert
        exception.Should().BeNull();
        user.Login.Should().Be(login);
        user.Password.Should().Be(password);
        user.Type.Should().Be(type);
    }

    [Fact(DisplayName = $"The {nameof(User)} cannot be created without password.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutPassword()
    {
        // Act
        var exception = Record.Exception(() => _ = new User(
            login: "login",
            password: null!,
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
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
            type: UserType.Customer));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}