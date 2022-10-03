using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.Logic.Abstractions;
/// <summary>
/// Описывает кэш сущностей с ключом.
/// </summary>
/// <typeparam name="TEntity">
/// Тип сущности, находящийся в кэше и имплементриует интерфейс <see cref="IKeyable{TKey}"/>.
/// </typeparam>
/// <typeparam name="TKey">
/// Тип ключа для сущности в кэше.
/// </typeparam>
public interface IKeyableCache<TEntity, TKey>: ICache<TEntity> where TEntity : IKeyable<TKey>
{
    /// <summary>
    /// Возвращет сущность <see cref="TEntity"/> из кэша по соответсвующему ключу <see cref="TKey"/>.
    /// </summary>
    /// <param name="key">
    /// Ключ сущности.  <see cref="TEntity"/>
    /// </param>
    /// <returns>
    /// Сущность соответсвующая ключу из кеша если такой сущности по ключу в кэше нет - <see langword="null"/>.
    /// </returns>
    public TEntity? GetByKey(TKey key);

    /// <summary>
    /// Определяется находится ли сущность в кэше по ключу.
    /// </summary>
    /// <param name="key">
    /// Ключ сущности.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, если сущность есть по ключу в кэше, иначе - <see langword="false"/>.
    /// </returns>
    public bool Contains(TKey key);
}
