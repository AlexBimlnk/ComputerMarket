﻿using General.Storage;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

using TUser = Models.User;

/// <summary xml:lang = "ru">
/// Репозиторий пользователей.
/// </summary>
public sealed class UsersRepository : IKeyableRepository<User, InternalID>
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

    private static TUser ConvertToStorageModel(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.Login,
        Email = user.Email,
        Password = user.Password.Value,
        UserTypeId = (short)user.Type
    };

    private User? ConvertFromStorageModel(TUser user)
    {
        if (!Enum.IsDefined(typeof(UserType), (int)user.UserTypeId))
        {
            _logger.LogWarning(
                "The user with user type id: {UserTypeId} can't be converted",
                user.UserTypeId);

            return null;
        }

        return new User(
            id: new InternalID(user.Id),
            user.Login,
            new Password(user.Password),
            email: "mail@mail.ru",
            (UserType)user.UserTypeId);
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
        .Select(x => ConvertFromStorageModel(x))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public User? GetByKey(InternalID key) =>
        _context.Users
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();
}