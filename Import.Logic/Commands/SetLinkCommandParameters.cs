using General.Logic.Commands;

using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры для команды на установку связи.
/// </summary>
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