using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

using PasswordClass = Market.Logic.Models.Password;

namespace Market.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Не указано имя пользователя")]
    [RegularExpression(AuthenticationData.ONLY_LETTERS_AND_NUMBERS_PATTERN,
         ErrorMessage = "Имя пользователя не может содержать специальные символы.")]
    [StringLength(AuthenticationData.LOGIN_MAX_LENGTH, MinimumLength = AuthenticationData.LOGIN_MIN_LENGTH, 
        ErrorMessage = "Имя пользователя может быть длиной от 6 до 20 символов")]
    [Display(Name = "Имя пользователя")]
    public string Login { get; set; } = default!;

    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    [RegularExpression(AuthenticationData.EMAIL_PATTERN,
         ErrorMessage = "Адрес электронной не соотвествует формату.")]
    [StringLength(AuthenticationData.EMAIL_MAX_LENGTH, MinimumLength = AuthenticationData.EMAIL_MIN_LENGTH,
        ErrorMessage = "Адрес электронной почты может быть длиной от 3 до 256 символов.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(AuthenticationData.ONLY_LETTERS_AND_NUMBERS_PATTERN,
         ErrorMessage = "Пароль не может содержать специальные символы.")]
    [StringLength(PasswordClass.PASSWORD_MAX_LENGTH, MinimumLength = PasswordClass.PASSWORD_MIN_LENGTH,
        ErrorMessage = "Пароль может быть длиной от 8 до 20 символов")]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = default!;
}