using General.Models;

namespace General.Storage;

/// <summary xml:lang = "ru">
/// Описывает репозиторий сущностей <see cref="TEntity"/> с ключом <see cref="TKey"/>.
/// </summary>
/// <typeparam name="TEntity" xml:lang = "ru">
/// Тип сущности, находящийся в репозитории и имплементрующей интерфейс <see cref="IKeyable{TKey}"/>.
/// </typeparam>
/// <typeparam name="TKey" xml:lang = "ru">
/// Тип ключа для сущности в репозитории.
/// </typeparam>
public interface IKeyableRepository<TEntity, TKey> : IRepository<TEntity>
    where TEntity : class, IKeyable<TKey>
{
    /// <summary xml:lang = "ru">
    /// Возвращет сущность <see cref="TEntity"/> из кэша по соответсвующему ключу <see cref="TKey"/>.
    /// </summary>
    /// <param name="key" xml:lang = "ru">
    /// Ключ сущности <see cref="TEntity"/>.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Сущность соответсвующая ключу из репозитория, если сущности с таким ключом нет - <see langword="null"/>.
    /// </returns>
    public TEntity? GetByKey(TKey key);
}
