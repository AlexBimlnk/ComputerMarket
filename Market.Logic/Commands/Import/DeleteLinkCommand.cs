using Market.Logic.Models;

namespace Market.Logic.Commands.Import;

/// <summary xml:lang = "ru">
/// Комманда на удаление связи.
/// </summary>
public sealed class DeleteLinkCommand : ImportCommand
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="DeleteLinkCommand"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
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
    public DeleteLinkCommand(
        CommandId id,
        ID externalItemId,
        Provider provider)
        : base(id, CommandType.DeleteLink)
    {
        ExternalItemId = externalItemId;
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор продукта у постащика.
    /// </summary>
    public ID ExternalItemId { get; }

    /// <summary xml:lang = "ru">
    /// Поставщик.
    /// </summary>
    public Provider Provider { get; }
}