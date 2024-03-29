﻿using General.Logic.Executables;
using General.Logic.Queries;

using Import.Logic.Models;

namespace Import.Logic.Queries;

/// <summary xml:lang = "ru">
/// Параметры для запроса на полуение всех связей.
/// </summary>
public sealed class GetLinksQueryParameters : QueryParametersBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="GetLinksQueryParameters"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    public GetLinksQueryParameters(ExecutableID id)
        : base(id)
    { }
}
