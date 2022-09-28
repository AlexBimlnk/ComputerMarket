using Import.Logic.Models;

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
public sealed record class SetLinkCommandParameters(InternalID InternalID, ExternalID ExternalID);