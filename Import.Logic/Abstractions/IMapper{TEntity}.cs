using Import.Logic.Models;

namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает маппер для соедения внещних сущностей с внтуреними.
/// </summary>
/// <typeparam name="TEntity" xml:lang = "ru">
/// Сущность которая будет маппиться.
/// </typeparam>
public interface IMapper<TEntity> where TEntity : IMappableEntity<InternalID, ExternalID>
{
    /// <summary xml:lang = "ru">
    /// Связывание сущности по ссылке.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">Сущность для маппинга.</param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Результат маппинга сущности типа <see cref="ValueTask{TResult}"/>.
    /// </returns>
    public ValueTask<TEntity> MapEntityAsync(TEntity entity, CancellationToken token = default);

    /// <summary xml:lang = "ru">
    /// Маппинг коллекции сущностей.
    /// </summary>
    /// <param name="entities" xml:lang = "ru">Коллекция сущостей.</param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Коллекция сущностей после маппинга типа <see cref="ValueTask{TResult}"/>.
    /// </returns>
    public ValueTask<IReadOnlyCollection<TEntity>> MapCollectionAsync(
        IReadOnlyCollection<TEntity> entities,
        CancellationToken token = default);

}
