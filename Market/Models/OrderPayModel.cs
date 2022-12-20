using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

namespace Market.Models;

public class OrderPayModel
{
    /// <summary>
    /// Счёт пользователя.
    /// </summary>
    [Required]
    [RegularExpression(PaymentTransactionsInformation.BANK_ACCOUNT_PATTERN, ErrorMessage = "Account is not mathc with pattern")]
    public string Account { get; set; } = default!;
}
