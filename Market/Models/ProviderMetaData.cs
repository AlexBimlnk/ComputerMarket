namespace Market.Models;

/// <summary xml:lang = "ru">
/// Представляет дополнительную информацию о поставщике.
/// </summary>
public class ProviderMetaData
{
    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public INN INN { get; set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; set; }

    /// <summary>
    /// Создает экземпляр типа <see cref="ProviderMetaData"/>.
    /// </summary>
    /// <param name="inn">Инн поставщика.</param>
    /// <param name="bankAccount">Счёт поставщика.</param>
    /// <exception cref="ArgumentException">Если <paramref name="bankAccount"/> имеет неверный формат.</exception>
    public ProviderMetaData(INN inn, string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
            throw new ArgumentException("Bank account can't be null or empty or contains only whitespaces", nameof(bankAccount));

        BankAccount = bankAccount;
        INN = inn;
    }
}
