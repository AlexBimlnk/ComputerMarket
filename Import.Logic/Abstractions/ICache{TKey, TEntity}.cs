using Newtonsoft.Json.Linq;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает кэш для сущности.
/// </summary>
/// <typeparam name="TEntity" xml:lang = "ru">
/// Тип сущности, находящийся в кэше.
/// </typeparam>
public interface ICache<TKey ,TEntity>
{
    /// <summary xml:lang = "ru">
    /// Добавляет сущность в кэш.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    public void Add(TKey key ,TEntity entity);

    /// <summary xml:lang = "ru">
    /// Добавляет сущности в кэш.
    /// </summary>
    /// <param name="entities" xml:lang = "ru">
    /// Коллекция сущностей.
    /// </param>
    public void AddRange(IEnumerable<KeyValuePair<TKey, TEntity>> entities);

    /// <summary xml:lang = "ru">
    /// Определяет, находится ли данная сущность в кэше.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если сущность есть в кэше, иначе - <see langword="false"/>.
    /// </returns>
    public bool Contains(TKey key);

    /// <summary xml:lang = "ru">
    /// Удаляет сущность из кэша.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    public void Delete(TKey key);

    public TEntity GetByKey(TKey key);
}