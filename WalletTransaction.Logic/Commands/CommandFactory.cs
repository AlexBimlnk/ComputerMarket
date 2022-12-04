using General.Logic.Commands;
using General.Logic.Executables;

namespace WalletTransaction.Logic.Commands;

/// <summary xml:lang = "ru">
/// Фабрика команд.
/// </summary>
public sealed class CommandFactory : ICommandFactory
{
    private readonly Func<CreateTransactionRequestCommandParameters, ICommand> _createRequestCommandFactory;
    private readonly Func<CancelTransactionRequestCommandParameters, ICommand> _cancelRequestCommandFactory;
    private readonly Func<FinishTransactionRequestCommandParameters, ICommand> _finishRequestCommandFactory;
    private readonly Func<RefundTransactionRequestCommandParameters, ICommand> _refundRequestCommandFactory;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandFactory"/>.
    /// </summary>
    /// <param name="createRequestCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="ExecutableID"/> и <see cref="CreateTransactionRequestCommand"/>
    /// команду типа <see cref="ICommand"/>.
    /// </param>
    /// <param name="cancelRequestCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="ExecutableID"/> и <see cref="CancelTransactionRequestCommand"/>
    /// команду типа <see cref="ICommand"/>.
    /// </param>
    /// <param name="finishRequestCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="ExecutableID"/> и <see cref="FinishTransactionRequestCommand"/>
    /// команду типа <see cref="ICommand"/>.
    /// </param>
    /// <param name="refundRequestCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="ExecutableID"/> и <see cref="RefundTransactionRequestCommand"/>
    /// команду типа <see cref="ICommand"/>.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из аргументов оказался <see langword="null"/>.
    /// </exception>
    public CommandFactory(
        Func<CreateTransactionRequestCommandParameters, ICommand> createRequestCommandFactory,
        Func<CancelTransactionRequestCommandParameters, ICommand> cancelRequestCommandFactory,
        Func<FinishTransactionRequestCommandParameters, ICommand> finishRequestCommandFactory,
        Func<RefundTransactionRequestCommandParameters, ICommand> refundRequestCommandFactory)
    {
        _createRequestCommandFactory = createRequestCommandFactory ?? throw new ArgumentNullException(nameof(createRequestCommandFactory));
        _cancelRequestCommandFactory = cancelRequestCommandFactory ?? throw new ArgumentNullException(nameof(cancelRequestCommandFactory));
        _finishRequestCommandFactory = finishRequestCommandFactory ?? throw new ArgumentNullException(nameof(finishRequestCommandFactory));
        _refundRequestCommandFactory = refundRequestCommandFactory ?? throw new ArgumentNullException(nameof(refundRequestCommandFactory));
    }

    /// <inheritdoc/>
    public ICommand Create(CommandParametersBase parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return parameters switch
        {
            CreateTransactionRequestCommandParameters transactionCommandParameters =>
                _createRequestCommandFactory(transactionCommandParameters),

            CancelTransactionRequestCommandParameters cancelRequestCommandParameters =>
                _cancelRequestCommandFactory(cancelRequestCommandParameters),

            FinishTransactionRequestCommandParameters finishRequestCommandParameters =>
                _finishRequestCommandFactory(finishRequestCommandParameters),

            RefundTransactionRequestCommandParameters refundRequestCommandParameters =>
                _refundRequestCommandFactory(refundRequestCommandParameters),

            _ => throw new ArgumentException(
                $"The command parameters type is unknown {parameters.GetType().Name}",
                nameof(parameters))
        };
    }
}
