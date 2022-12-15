using Market.Logic.Models;
using System.ComponentModel.DataAnnotations;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Модель представления для информации о новом поставщике.
/// </summary>
public class ProviderRegisterViewModel
{
    /// <summary xml:lang = "ru">
    /// Название поставщика.
    /// </summary>
    [Required(ErrorMessage = "Не указано название поставщика.")]
    [Display(Name = "Название")]
    public string Name { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// Счёт поставщика.
    /// </summary>
    [Required(ErrorMessage = "Не указан номер счёта в банке")]
    [RegularExpression(PaymentTransactionsInformation.BANK_ACCOUNT_PATTERN,
         ErrorMessage = "Указаный номер не соотвествует формату.")]
    [Display(Name = "Номер счёта")]
    public string BankAccount { get; set; } = default!;

    /// <summary xml:lang = "ru">
    /// ИНН поставщика.
    /// </summary>
    [Required(ErrorMessage = "Не указан ИНН")]
    [RegularExpression(PaymentTransactionsInformation.INN_PATTERN,
         ErrorMessage = "ИНН не соотвествует формату.")]
    [Display(Name = "ИНН")]
    public string INN { get; set; } = default!;
}
