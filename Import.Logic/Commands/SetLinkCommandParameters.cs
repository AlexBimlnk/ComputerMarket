using Import.Logic.Abstractions.Commands;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры для команды на установку связи.
/// </summary>
/// <param name="InternalID" xml:lang = "ru">
/// Внутренний идентификатор для которого устанавливается связь.
/// </param>
/// <param name="ExternalID" xml:lang = "ru">
/// Внеший идентификатор, который нужно привязать ко внутреннему.
/// </param>
public sealed class SetLinkCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="SetLinkCommandParameters"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <param name="internalID" xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </param>
    /// <param name="externalID" xml:lang = "ru">
    /// Внешний идентификатор.
    /// </param>
    public SetLinkCommandParameters(CommandID id, InternalID internalID, ExternalID externalID)
        : base(id)
    {
        InternalID = internalID;
        ExternalID = externalID;
    }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    public InternalID InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    public ExternalID ExternalID { get; }
}