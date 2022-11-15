﻿using General.Logic.Commands;
using General.Transport;

using WalletTransaction.Logic.Transport;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Команда полного рассчета запроса на проведение транзакций.
/// </summary>
public sealed class FinishTransactionRequestCommand : CommandBase, ICommand
{
    private readonly ISender<TransactionSenderConfiguration, ITransactionsRequest> _sender;
    private readonly ITransactionRequestCache _requestCache;
    private readonly FinishTransactionRequestCommandParameters _parameters;

    /// <summary xml:lang = "ru">
    /// Создаёт новый экземпляр типа <see cref="FinishTransactionRequestCommand"/>.
    /// </summary>
    /// <param name="parameters" xml:lang = "ru">
    /// Параметры команды.
    /// </param>
    /// <param name="sender" xml:lang = "ru">
    /// Отправитель запросов.
    /// </param>
    /// <param name="requestCache" xml:lang = "ru">
    /// Кэш запросов.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public FinishTransactionRequestCommand(
        FinishTransactionRequestCommandParameters parameters,
        ISender<TransactionSenderConfiguration, ITransactionsRequest> sender,
        ITransactionRequestCache requestCache)
    {
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _requestCache = requestCache ?? throw new ArgumentNullException(nameof(requestCache));
    }

    /// <inheritdoc/>
    public override CommandID Id => _parameters.Id;

    protected override async Task ExecuteCoreAsync()
    {
        var request = _requestCache.GetByKey(_parameters.TransactionRequestId);

        if (request is null)
            throw new InvalidOperationException(
                $"Transaction request with id {_parameters.TransactionRequestId} is not exists.");

        await _sender.SendAsync(request);

        _requestCache.Delete(request);
    }
}
