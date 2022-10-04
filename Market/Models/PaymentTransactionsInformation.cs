namespace Market.Models;

/// <summary xml:lang = "ru">
/// Представляет информацию для платежных операций.
/// </summary>
public class PaymentTransactionsInformation
{
    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public INN INN { get; private set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; private set; }

    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="PaymentTransactionsInformation"/>.
    /// </summary>
    /// <param name="inn" xml:lang = "ru">Инн поставщика.</param>
    /// <param name="bankAccount" xml:lang = "ru">Счёт поставщика.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">Если <paramref name="bankAccount"/> имеет неверный формат.</exception>
    public PaymentTransactionsInformation(INN inn, string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
            throw new ArgumentException("Bank account can't be null or empty or contains only whitespaces", nameof(bankAccount));

        BankAccount = bankAccount;
        INN = inn;
    }
}
