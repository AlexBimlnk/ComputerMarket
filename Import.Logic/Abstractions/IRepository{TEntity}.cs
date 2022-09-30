namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает репозиторий сущностей.
/// </summary>
/// <typeparam name="TEntity" xml:lang = "ru">
/// Тип сущности находящейся в репозитории.
/// </typeparam>
public interface IRepository<TEntity>
{
    /// <summary xml:lang = "ru">
    /// Возвращает коллекцию сущностей, находящихся в репозитории.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Коллекция типа <see cref="IEnumerable{T}"/>.
    /// </returns>
    public Task<IEnumerable<TEntity>> GetEntitiesAsync();

    /// <summary xml:lang = "ru">
    /// Определяет, находится ли данная сущность в репозитории.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если сущность есть в репозитории, иначе - <see langword="false"/>.
    /// </returns>
    public Task<bool> ContainsAsync(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Добавляет сущность в репозиторий.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task AddAsync(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Удаляет сущность из репозитория.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task DeleteAsync(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Сохраняет все накопленные команды.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task SaveAsync();
}
