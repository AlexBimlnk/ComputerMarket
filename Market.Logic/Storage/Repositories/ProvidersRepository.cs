using System.Collections;
using System.Security.Cryptography.X509Certificates;

using Market.Logic.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Market.Logic.Storage.Repositories;

using TProvider = Models.Provider;
using TUser = Models.User;

public sealed class ProvidersRepository : IProvidersRepository
{
    private readonly IRepositoryContext _context;
    private readonly ILogger<ProvidersRepository> _logger;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ProvidersRepository"/>.
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
    public ProvidersRepository(
        IRepositoryContext context,
        ILogger<ProvidersRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
   

    /// <inheritdoc/>
    public async Task AddAsync(Provider entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        await _context.Providers.AddAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ContainsAsync(Provider entity, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        token.ThrowIfCancellationRequested();

        return await _context.Providers.ContainsAsync(ConvertToStorageModel(entity), token)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Delete(Provider entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var storageProvider = _context.Providers.SingleOrDefault(x => x.Id == ConvertToStorageModel(entity).Id);

        if (storageProvider is null)
            return;

        _context.Providers.Remove(storageProvider);
    }

    /// <inheritdoc/>
    public IEnumerable<Provider> GetEntities() =>
        _context.Providers
        .AsEnumerable()
        .Select(x => ConvertFromStorageModel(x))
        .Where(x => x != null)!;

    /// <inheritdoc/>
    public void Update(Provider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        var storageProvider = ConvertToStorageModel(provider);

        var newStorageProvider = _context.Providers.SingleOrDefault(x => x.Id == storageProvider.Id);

        if (newStorageProvider is null)
            return;

        newStorageProvider.Margin = storageProvider.Margin;
        newStorageProvider.Inn= storageProvider.Inn;
        newStorageProvider.BankAccount = storageProvider.BankAccount;
        newStorageProvider.IsAproved = storageProvider.IsAproved;

        _context.Providers.Update(newStorageProvider);
    }

    /// <inheritdoc/>
    public void Save() => _context.SaveChanges();

    /// <inheritdoc/>
    public Provider? GetByKey(ID key) =>
        _context.Providers
            .Where(x => x.Id == key.Value)
            .Select(x => ConvertFromStorageModel(x))
            .SingleOrDefault();

    /// <inheritdoc/>
    public IEnumerable<User> GetAgents(Provider provider)
    {
        var storage = ConvertToStorageModel(provider);

        if (!provider.IsAproved)
        {
            throw new InvalidOperationException("Given provider can't hava agents");
        }

        var agents = _context.Users
            .Where(x => x.ProvidersAgent != null && x.ProvidersAgent.ProviderId == storage.Id)
            .ToList()
            .Select(x => ConvertFromStorageModel(x, _logger))
            .Where(x => x != null);

        return agents!;
    }

    /// <inheritdoc/>
    public void AddAgent(ProviderAgent agent)
    {
        var storageProvider = ConvertToStorageModel(agent.Provider);
        var storageUser = ConvertToStorageModel(agent.Agent);

        var currentUser = _context.Users.SingleOrDefault(x => x.Id == storageUser.Id);
        var currentProvider = _context.Providers.SingleOrDefault(x => x.Id == storageProvider.Id);

        if (currentUser is null || currentProvider is null)
        {
            return;
        }

        if (currentUser.UserTypeId != ((int)UserType.Agent))
        {
            return;
        }

        currentUser.ProvidersAgent = new Models.ProviderAgent()
        {
            Provider = currentProvider,
            UserId = storageUser.Id,
            ProviderId = storageProvider.Id
        };

        _context.Users.Update(currentUser);
    }

    /// <inheritdoc/>
    public void RemoveAgent(ProviderAgent agent)
    {
        var storageProvider = ConvertToStorageModel(agent.Provider);
        var storageUser = ConvertToStorageModel(agent.Agent);

        var currentUser = _context.Users.SingleOrDefault(x => x.Id == storageUser.Id);
        var currentProvider = _context.Providers.SingleOrDefault(x => x.Id == storageProvider.Id);

        if (currentUser is null || currentProvider is null)
        {
            return;
        }

        if (currentUser.UserTypeId != ((int)UserType.Agent) || currentUser.ProvidersAgent == null)
        {
            return;
        }

        currentUser.ProvidersAgent = null;

        _context.Users.Update(currentUser);
    }

    /// <inheritdoc/>
    public ProviderAgent GetAgent(User user)
    {
        var curUser = _context.Users.SingleOrDefault(x => x.Id == user.Key.Value);

        if (curUser is null || curUser.ProvidersAgent is null)
            return null!;

        return new ProviderAgent(ConvertFromStorageModel(curUser, _logger)!, ConvertFromStorageModel(curUser.ProvidersAgent.Provider));
    }

    #region Converters

    private static TProvider ConvertToStorageModel(Provider provider) => new()
    {
        Id = provider.Key.Value,
        Name = provider.Name,
        Margin = provider.Margin.Value,
        Inn = provider.PaymentTransactionsInformation.INN,
        BankAccount = provider.PaymentTransactionsInformation.BankAccount,
        IsAproved = provider.IsAproved
    };

    private static Provider ConvertFromStorageModel(TProvider provider) =>
        new Provider(
            new ID(provider.Id),
            provider.Name,
            new Margin(provider.Margin),
            new PaymentTransactionsInformation(provider.Inn, provider.BankAccount))
        {
            IsAproved= provider.IsAproved
        };

    private static TUser ConvertToStorageModel(User user) => new()
    {
        Id = user.Key.Value,
        Login = user.AuthenticationData.Login,
        Email = user.AuthenticationData.Email,
        Password = user.AuthenticationData.Password.Value,
        UserTypeId = (short)user.Type
    };

    private static User? ConvertFromStorageModel(TUser user, ILogger<ProvidersRepository> _logger)
    {
        if (!Enum.IsDefined(typeof(UserType), (int)user.UserTypeId))
        {
            _logger.LogWarning(
                "The user with user type id: {UserTypeId} can't be converted",
                user.UserTypeId);
            return null;
        }

        return new User(
            id: new ID(user.Id),
            new AuthenticationData(
                user.Email,
                new Password(user.Password),
                user.Login),
            (UserType)user.UserTypeId);
    }

    #endregion
}
