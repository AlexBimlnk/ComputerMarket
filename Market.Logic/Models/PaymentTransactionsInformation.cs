using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет информацию для платежных операций.
/// </summary>
public class PaymentTransactionsInformation
{
    private const string INN_PATTERN = @"^[0-9]{10}$";
    private const string BANK_ACCOUNT_PATTERN = @"^[0-9]{20}$";

    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public string INN { get; private set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; private set; }

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="PaymentTransactionsInformation"/>.
    /// </summary>
    /// <param name="inn" xml:lang = "ru">Инн поставщика.</param>
    /// <param name="bankAccount" xml:lang = "ru">Счёт поставщика.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="bankAccount"/> или <paramref name="inn"/> имеет неверный формат.
    /// </exception>
    public PaymentTransactionsInformation(string inn, string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
            throw new ArgumentException($"{nameof(bankAccount)} can't be null or empty or contains only whitespaces", nameof(bankAccount));

        if (!Regex.IsMatch(bankAccount, BANK_ACCOUNT_PATTERN))
            throw new ArgumentException($"Given {nameof(bankAccount)} not match with account format");

        BankAccount = bankAccount;

        if (string.IsNullOrWhiteSpace(inn))
            throw new ArgumentException($"{nameof(inn)} can't be null or empty or contains only whitespaces", nameof(inn));

        if (!Regex.IsMatch(inn, INN_PATTERN))
            throw new ArgumentException($"Given {nameof(inn)} not match with INN format");

        INN = inn;
    }
}