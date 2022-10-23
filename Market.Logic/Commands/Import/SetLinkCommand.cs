using Market.Logic.Models;

namespace Market.Logic.Commands.Import;

/// <summary xml:lang = "ru">
/// Команда установка связи.
/// </summary>
public sealed class SetLinkCommand : ImportCommand
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommand"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <param name="internalItemId" xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </param>
    /// <param name="externalItemId" xml:lang = "ru">
    /// Идентификатор продукта поставщика.
    /// </param>
    /// <param name="provider" xml:lang = "ru">
    /// Поставщик.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="provider"/> оказался <see langword="null"/>.
    /// </exception>
    public SetLinkCommand(
        CommandId id,
        InternalID internalItemId,
        InternalID externalItemId,
        Provider provider) 
        : base(id)
    {
        InternalItemId = internalItemId;
        ExternalItemId = externalItemId;
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public InternalID InternalItemId { get; }
    public InternalID ExternalItemId { get; }
    public Provider Provider { get; }
}
