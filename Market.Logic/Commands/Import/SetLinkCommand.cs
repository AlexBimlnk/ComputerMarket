using General.Logic.Commands;

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
        CommandID id,
        ID internalItemId,
        ID externalItemId,
        Provider provider)
        : base(id, CommandType.SetLink)
    {
        InternalItemId = internalItemId;
        ExternalItemId = externalItemId;
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор внутреннего продукта.
    /// </summary>
    public ID InternalItemId { get; }

    /// <summary xml:lang = "ru">
    /// Идентификатор продукта у постащика.
    /// </summary>
    public ID ExternalItemId { get; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; }
}
