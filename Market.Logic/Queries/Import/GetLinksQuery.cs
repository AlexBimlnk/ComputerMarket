﻿using General.Logic.Executables;

namespace Market.Logic.Queries.Import;

/// <summary xml:lang = "ru">
/// Запрос на получение всех связей.
/// </summary>
public sealed class GetLinksQuery : QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый объект типа <see cref="GetLinksQuery"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор запроса.
    /// </param>
    /// <param name="type" xml:lang = "ru">
    /// Тип запроса.
    /// </param>
    public GetLinksQuery(ExecutableID id)
        : base(id, QueryType.GetLinks)
    {
    }
}
