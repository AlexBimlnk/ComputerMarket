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
    /// Возвращает сущность по её идентификатору.
    /// </summary>
    /// <param name="id" xml:lang = "ru">
    /// Идентификатор сущности.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Объект типа <typeparamref name="TEntity"/>.
    /// </returns>
    public Task<TEntity> GetById(int id);

    /// <summary xml:lang = "ru">
    /// Возвращает коллекцию сущностей, находящихся в репозитории.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// Коллекция типа <see cref="IEnumerable{T}"/>.
    /// </returns>
    public Task<IEnumerable<TEntity>> GetEntities();

    /// <summary xml:lang = "ru">
    /// Определяет, находится ли данная сущность в репозитории.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/>, если сущность есть в репозитории, иначе - <see langword="false"/>.
    /// </returns>
    public Task<bool> Contains(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Добавляет сущность в репозиторий.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task Add(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Удаляет сущность из репозитория.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Сущность.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task Delete(TEntity entity);

    /// <summary xml:lang = "ru">
    /// Сохраняет все накопленные команды.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task"/>.
    /// </returns>
    public Task Save();
}
