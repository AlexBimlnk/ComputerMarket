using General.Logic.Executables;

namespace Market.Logic.Queries;

/// <summary xml:lang = "ru">
/// Абстрактный базовый запрос.
/// </summary>
public abstract class QueryBase
{
    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="QueryBase"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор команды.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="id"/> оказался <see langword="null"/>.
    /// </exception>
    protected QueryBase(ExecutableID id, QueryType type)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

        if (!Enum.IsDefined(typeof(QueryType), type))
            throw new ArgumentException("Given unknown command type", nameof(type));
        Type = type;
    }

    /// <summary xml:lang = "ru">
    /// Идентификатор команды.
    /// </summary>
    public ExecutableID Id { get; }

    /// <summary xml:lang = "ru">
    /// Тип команды.
    /// </summary>
    public QueryType Type { get; }
}
