using Market.Logic.Models;

namespace Market.Logic.Tests;

public class PasswordTests
{
    [Fact(DisplayName = $"The {nameof(Password)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var passwordString = "password";
        Password password = null!;

        // Act
        var exception = Record.Exception(() => password = new Password(passwordString));

        // Assert
        exception.Should().BeNull();
        password.Value.Should().Be(passwordString);
    }

    [Theory(DisplayName = $"The {nameof(Password)} cannot be created when given password string has incorrect format.")]
    [Trait("Category", "Unit")]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("\t \n\r ")]
    public void CanNotCreateWhenPasswordIncorrect(string passwordString)
    {
        // Act
        var exception = Record.Exception(() => _ = new Password(passwordString));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }
}