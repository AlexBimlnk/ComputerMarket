using System.Text.RegularExpressions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Представляет информацию для платежных операций.
/// </summary>
public class PaymentTransactionsInformation
{
    private readonly static string s_innPattern = @"^[0-9]{10}$";

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
            throw new ArgumentException("Bank account can't be null or empty or contains only whitespaces", nameof(bankAccount));

        BankAccount = bankAccount;

        if (string.IsNullOrWhiteSpace(inn))
            throw new ArgumentException("INN can't be null or empty or contains only whitespaces", nameof(inn));

        if (!Regex.IsMatch(inn, s_innPattern))
            throw new ArgumentException("Given inn not match with INN format");

        INN = inn;
    }
}