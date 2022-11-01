using General.Models;

namespace General.Storage;

/// <summary xml:lang = "ru">
/// Описывает кэш сущностей с ключом.
/// </summary>
/// <typeparam name="TEntity" xml:lang = "ru">
/// Тип сущности, находящийся в кэше и имплементриует интерфейс <see cref="IKeyable{TKey}"/>.
/// </typeparam>
/// <typeparam name="TKey" xml:lang = "ru">
/// Тип ключа для сущности в кэше.
/// </typeparam>
public interface IKeyableCache<TEntity, TKey> : ICache<TEntity> where TEntity : IKeyable<TKey>
{
    /// <summary xml:lang = "ru">
    /// Возвращет сущность <see cref="TEntity"/> из кэша по соответсвующему ключу <see cref="TKey"/>.
    /// </summary>
    /// <param name="key" xml:lang = "ru">
    /// Ключ сущности <see cref="TEntity"/>.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Сущность соответсвующая ключу из кеша если такой сущности по ключу в кэше нет - <see langword="null"/>.
    /// </returns>
    public TEntity? GetByKey(TKey key);

    /// <summary xml:lang = "ru">
    /// Определяется находится ли сущность в кэше по ключу.
    /// </summary>
    /// <param name="key" xml:lang = "ru">
    /// Ключ сущности.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если сущность есть по ключу в кэше, иначе - <see langword="false"/>.
    /// </returns>
    public bool Contains(TKey key);
}
