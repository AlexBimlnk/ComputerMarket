using Market.Logic.Models;
using System.ComponentModel.DataAnnotations;

namespace Market.Models;

public class NewAgentViewModel
{
    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    [Required(ErrorMessage = "Почта не может быть пустой")]
    [RegularExpression(AuthenticationData.EMAIL_PATTERN,
         ErrorMessage = "Адрес электронной не соотвествует формату.")]
    public string Email { get; set; } = null!;
}
