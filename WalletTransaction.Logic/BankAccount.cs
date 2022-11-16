using System.Text.RegularExpressions;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Банковский счёт.
/// </summary>
public sealed record class BankAccount
{
    private const string BANK_ACCOUNT_PATTERN = @"^[0-9]{20}$";

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="BankAccount"/>.
    /// </summary>
    /// <param name="bankAccount" xml:lang = "ru">
    /// Номер счёта.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="bankAccount"/> имеет неверный формат.
    /// </exception>
    public BankAccount(string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
            throw new ArgumentException(
                $"{nameof(bankAccount)} can't be null or empty or contains only whitespaces", 
                nameof(bankAccount));

        if (!Regex.IsMatch(bankAccount, BANK_ACCOUNT_PATTERN))
            throw new ArgumentException(
                $"Given {nameof(bankAccount)} not match with account format",
                nameof(bankAccount));

        Value = bankAccount;
    }

    /// <summary xml:lang = "ru">
    /// Банковский счёт.
    /// </summary>
    public string Value { get; }
}
