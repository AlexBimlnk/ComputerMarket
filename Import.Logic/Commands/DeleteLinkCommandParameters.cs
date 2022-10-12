﻿using Import.Logic.Abstractions.Commands;
using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры для команды на удаление связи.
/// </summary>
public sealed class DeleteLinkCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="DeleteLinkCommandParameters"/>.
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
    public DeleteLinkCommandParameters(CommandID id, InternalID internalID, ExternalID externalID) : base(id)
    {
        ExternalID = externalID;
        InternalID = internalID;
    }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор.
    /// </summary>
    public ExternalID ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор.
    /// </summary>
    public InternalID InternalID { get; }
}
