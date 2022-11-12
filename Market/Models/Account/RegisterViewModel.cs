using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

using PasswordClass = Market.Logic.Models.Password;

namespace Market.Models.Account;

/// <summary xml:lang = "ru">
/// Модель предсталвения для регистрации пользователя.
/// </summary>
public class RegisterViewModel
{
    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указано имя пользователя")]
    [RegularExpression(AuthenticationData.LOGIN_PATTERN,
         ErrorMessage = "Имя пользователя должно содержать только цифры и латинские символы, и иметь длину от 6 до 20 символов.")]
    [Display(Name = "Имя пользователя")]
    public string Login { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Электронная почта пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    [RegularExpression(AuthenticationData.EMAIL_PATTERN,
         ErrorMessage = "Адрес электронной не соотвествует формату.")]
    public string Email { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Пароль пользователя.
    /// </summary>
    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(PasswordClass.PASSWORD_PATTERN,
         ErrorMessage = "Пароль может содержать только цифры и латинские символы, и иметь длинну от 8 до 20 символов.")]
    public string Password { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Повторенный пароль пользователя.
    /// </summary>
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = default!;
}