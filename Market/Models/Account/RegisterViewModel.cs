using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

namespace Market.Models.Account;

public class RegisterModel
{
    [Required(ErrorMessage = "Не указано имя пользователя")]
    [RegularExpression(User.ONLY_LETTERS_AND_NUMBERS_PATTERN,
         ErrorMessage = "Имя пользователя не может содержать специальные символы.")]
    [StringLength(User.LOGIN_MAX_LENGTH, MinimumLength = User.LOGIN_MIN_LENGTH, 
        ErrorMessage = "Имя пользователя может быть длиной от 6 до 20 символов")]
    [Display(Name = "Имя пользователя")]
    public string Login { get; set; } = default!;

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
    [StringLength(User.PASSWORD_MAX_LENGTH, MinimumLength = User.PASSWORD_MIN_LENGTH,
        ErrorMessage = "Пароль может быть длиной от 8 до 20 символов")]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}