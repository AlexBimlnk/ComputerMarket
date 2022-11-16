using General.Models;

namespace WalletTransaction.Logic;

/// <summary xml:lang = "ru">
/// Запрос на проведение транзаций.
/// </summary>
public sealed class TransactionRequest : IKeyable<InternalID>
{
    /// <summary xml:lang = "ru">
    /// Создает новый объект типа <see cref="TransactionRequest"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    /// <param name="transactions" xml:lang = "ru">
    /// Транзакции, которые нужно провести в рамках данного запроса.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="transactions"/> оказался <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="transactions"/> пусты.
    /// </exception>
    public TransactionRequest(
        InternalID id,
        IReadOnlyCollection<Transaction> transactions)
    {
        Id = id;
        Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));

        if (!Transactions.Any())
        {
            throw new ArgumentException("Request can't have empty transactions");
        }
    }

    /// <inheritdoc/>
    public InternalID Id { get; }

    /// <inheritdoc/>
    public InternalID Key => Id;

    /// <inheritdoc/>
    public IReadOnlyCollection<Transaction> Transactions { get; }
}
