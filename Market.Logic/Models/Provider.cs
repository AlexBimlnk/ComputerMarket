using General.Models;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Модель поставщика.
/// </summary>
public sealed class Provider : IEquatable<Provider>, IKeyable<ID>
{
    /// <summary xml:lang = "ru">
    /// Создает экземпляр типа <see cref="Provider"/>.
    /// </summary>
    /// <param name="id"xml:lang = "ru">Индетифкатор првайдера.</param>
    /// <param name="name" xml:lang = "ru">Название поставщика.</param>
    /// <param name="margin" xml:lang = "ru">Маржа поставщика.</param>
    /// <param name="paymentTransactionsInformation" xml:lang = "ru">Дполнительная информация об поставщике.</param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="paymentTransactionsInformation"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Если <paramref name="name"/> не соответсвует уставновленному формату.
    /// </exception>
    public Provider(
        ID id,
        string name,
        Margin margin,
        PaymentTransactionsInformation paymentTransactionsInformation)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name can't be null or empty or contains only whitespaces", nameof(name));
        Name = name;

        Margin = margin;

        PaymentTransactionsInformation = paymentTransactionsInformation ?? throw new ArgumentNullException(nameof(paymentTransactionsInformation));

        Key = id;
    }

    /// <summary xml:lang = "ru">
    ///  Название поставщика.
    /// </summary>
    public string Name { get; private set; }

    /// <summary xml:lang = "ru">
    /// Заданная маржа поставщика.
    /// </summary>
    public Margin Margin { get; }

    /// <summary xml:lang = "ru">
    /// Дополнительная информация об поставщике.
    /// </summary>
    public PaymentTransactionsInformation PaymentTransactionsInformation { get; private set; }

    /// <summary xml:lang="ru">
    /// Подтвержден ли провайдер.
    /// </summary>
    public bool IsAproved { get; set; } = false;

    /// <inheritdoc/>
    public ID Key { get; }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Key, PaymentTransactionsInformation, Name, Margin, IsAproved);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Provider provider && Equals(provider);

    /// <inheritdoc/>
    public bool Equals(Provider? other) =>
        Key.Equals(other?.Key) &&
        Margin.Equals(other?.Margin) &&
        Name.Equals(other?.Name) &&
        PaymentTransactionsInformation.Equals(other?.PaymentTransactionsInformation) &&
        IsAproved.Equals(other?.IsAproved);

}
