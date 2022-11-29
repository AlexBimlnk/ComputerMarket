﻿using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

/// <summary xml:lang = "ru">
/// Репозиторий пользователей.
/// </summary>
public sealed class UsersRepository : RepositoryHelper, IUsersRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<UsersRepository> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="UsersRepository"/>.
    /// </summary>
    /// <param name="context" xml:lang = "ru">
    /// Контекст репозиториев БД.
    /// </param>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из параметров равен <see langword="null"/>.
    /// </exception>
    public UsersRepository(
        IRepositoryContext context,
        ILogger<UsersRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddAsync(User entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Users.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(User entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Users.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Users.Remove(ConvertToStorageModel(entity));
    }

    /// <inheritdoc/>
    public IEnumerable<User> GetEntities() =>
        _context.Users
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x, _logger))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public User? GetByKey(ID key) =>
        _context.Users
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x, _logger))
            .SingleOrDefault();

    /// <inheritdoc/>
    public User? GetByEmail(string email)
    {
        var user = _context.Users
            .SingleOrDefault(x => x.Email == email);

        if (user is null)
            return null;

        return ConvertFromStorageModel(user, _logger);
    }

    /// <inheritdoc/>
    public bool IsCanAuthenticate(AuthenticationData data, out User user)
    {
        ArgumentNullException.ThrowIfNull(data);

        user = null!;
        var foundUser = GetByEmail(data.Email);

        if (foundUser is null || !IsPasswordMatch(foundUser.Key, data.Password))
        {
            return false;
        }

        user = foundUser;
        return true;
    }

    /// <inheritdoc/>
    public bool IsPasswordMatch(ID id, Password password) =>
        _context.Users
            .SingleOrDefault(x => x.Id == id.Value) switch
        {
            null => false,
            var user => user.Password == password.Value
        };
}
