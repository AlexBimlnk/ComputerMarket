using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

using PasswordClass = Market.Logic.Models.Password;

namespace Market.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    [RegularExpression(User.EMAIL_PATTERN,
         ErrorMessage = "Адрес электронной не соотвествует формату.")]
    [StringLength(User.EMAIL_MAX_LENGTH, MinimumLength = User.EMAIL_MIN_LENGTH,
        ErrorMessage = "Адрес электронной почты может быть длиной от 3 до 256 символов.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(User.ONLY_LETTERS_AND_NUMBERS_PATTERN,
         ErrorMessage = "Пароль не может содержать специальные символы.")]
    [StringLength(PasswordClass.PASSWORD_MAX_LENGTH, MinimumLength = PasswordClass.PASSWORD_MIN_LENGTH,
        ErrorMessage = "Пароль может быть длиной от 8 до 20 символов")]
    public string Password { get; set; } = default!;
}
