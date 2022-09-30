using Import.Logic.Storage.Models;

using Microsoft.EntityFrameworkCore;

namespace Import.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Описывет контекст БД для репозиториев.
/// </summary>
public interface IRepositoryContext
{
    /// <summary xml:lang = "ru">
    /// Связи.
    /// </summary>
    public DbSet<Link> Links { get; }

    /// <summary xml:lang = "ru">
    /// Истории.
    /// </summary>
    public DbSet<History> Histories { get; }

    /// <summary xml:lang = "ru">
    /// Поставщики.
    /// </summary>
    public DbSet<Provider> Providers { get; }

    /// <summary xml:lang = "ru">
    /// Сохраняет текущие изменения.
    /// </summary>
    public void SaveChanges();
}
