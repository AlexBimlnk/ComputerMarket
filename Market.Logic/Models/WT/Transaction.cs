namespace Market.Logic.Models.WT;

/// <summary xml:lang = "ru">
/// Банковская транзакция
/// </summary>
public sealed class Transaction
{
    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="Transaction"/>.
    /// </summary>
    /// <param name="fromAccount" xml:lang = "ru">
    /// Счет отправителя.
    /// </param>
    /// <param name="toAccount" xml:lang = "ru">
    /// Счет получателя.
    /// </param>
    /// <param name="transferBalance" xml:lang = "ru">
    /// Передаваемый баланс.
    /// </param>
    /// <param name="heldBalance" xml:lang = "ru">
    /// Удерживаемый баланс.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров оказался <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException" xml:lang = "ru">
    /// Когда <paramref name="transferBalance"/> оказался меньше или равным нулю.
    /// </exception>
    public Transaction(
        string fromAccount,
        string toAccount,
        decimal transferBalance,
        decimal heldBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(fromAccount))
            throw new ArgumentException("'From' account can't be null, empty or has only whitespaces");
        From = fromAccount;

        if (string.IsNullOrWhiteSpace(toAccount))
            throw new ArgumentException("'To' account can't be null, empty or has only whitespaces");
        To = toAccount;

        if (transferBalance <= 0)
            throw new ArgumentOutOfRangeException(nameof(transferBalance));

        TransferBalance = transferBalance;

        if (heldBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(heldBalance));

        HeldBalance = heldBalance;
    }

    /// <summary xml:lang = "ru">
    /// Счет отправителя.
    /// </summary>
    public string From { get; }

    /// <summary xml:lang = "ru">
    /// Счет получателя.
    /// </summary>
    public string To { get; }

    /// <summary xml:lang = "ru">
    /// Передаваемый баланс.
    /// </summary>
    public decimal TransferBalance { get; }

    /// <summary xml:lang = "ru">
    /// Удерживаемый баланс.
    /// </summary>
    public decimal HeldBalance { get; }
}
