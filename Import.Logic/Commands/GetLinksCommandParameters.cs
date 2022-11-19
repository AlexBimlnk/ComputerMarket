using General.Logic.Commands;

using Import.Logic.Models;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Параметры для команды на полуение всех связей.
/// </summary>
public sealed class GetLinksCommandParameters : CommandParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GetLinksCommandParameters"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    public GetLinksCommandParameters(CommandID id)
        : base(id)
    { } 
}
