namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Банковская транзакция
/// </summary>
public sealed class Transaction
{
    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="Transaction"/>.
    /// </summary>
    /// <param name="from" xml:lang = "ru">
    /// Счет отправителя.
    /// </param>
    /// <param name="to" xml:lang = "ru">
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
        BankAccount from, 
        BankAccount to, 
        decimal transferBalance,
        decimal heldBalance = 0)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));
        To = to ?? throw new ArgumentNullException(nameof(to));

        if (transferBalance <= 0)
            throw new ArgumentOutOfRangeException(nameof(transferBalance));

        TransferBalance = transferBalance;

        if (heldBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(heldBalance));

        TransferBalance = heldBalance;
    }

    /// <summary xml:lang = "ru">
    /// Счет отправителя.
    /// </summary>
    public BankAccount From { get; }

    /// <summary xml:lang = "ru">
    /// Счет получателя.
    /// </summary>
    public BankAccount To { get; }

    /// <summary xml:lang = "ru">
    /// Передаваемый баланс.
    /// </summary>
    public decimal TransferBalance { get; }

    /// <summary xml:lang = "ru">
    /// Удерживаемый баланс.
    /// </summary>
    public decimal HeldBalance { get; }

    /// <summary xml:lang = "ru">
    /// Флаг, указывающий состояние завершения выполнения транзакции.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary xml:lang = "ru">
    /// Завершает транзакцию.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Если транзакция уже была завершена.
    /// </exception>
    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Transaction already is completed.");

        IsCompleted = true;
    }
}
