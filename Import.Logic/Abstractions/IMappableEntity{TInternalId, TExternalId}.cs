using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает сущность передаваемую в маппер.
/// </summary>
/// <typeparam name="TInternalId" xml:lang = "ru">
/// Внутрений индетификатор сущности для маппинга.
/// </typeparam>
/// <typeparam name="TExternalId" xml:lang = "ru">
/// Внешний индетификатор сущности для маппинга.
/// </typeparam>
public interface IMappableEntity<TInternalId, TExternalId>
{
    /// <summary xml:lang = "ru">
    /// Указывает была ли выполнена операция маппинга для этой сущности.
    /// </summary>
    public bool IsMapped { get; }

    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор сущности.
    /// </summary>
    public TInternalId InternalID { get; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор сущности.
    /// </summary>
    public TExternalId ExternalID { get; }

    /// <summary xml:lang = "ru">
    /// Устанавливает связь с внутренней сущностью.
    /// </summary>
    /// <param name="internalID" xml:lang = "ru">
    /// Идентификатор внутренней сущности.
    /// </param>
    public void MapTo(TInternalId mapId);
}
